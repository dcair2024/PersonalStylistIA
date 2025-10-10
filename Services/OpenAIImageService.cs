using System.Text;
using System.Text.Json;

public class OpenAIImageService : IOpenAIImageService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey; 
    private readonly ILogger<OpenAIImageService> _logger;

    public OpenAIImageService(HttpClient httpClient, IConfiguration config, ILogger<OpenAIImageService> logger)
    {
        _httpClient = httpClient;
        _apiKey = config["OpenAI:ApiKey"];
        _logger = logger;

        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
        _httpClient.Timeout = TimeSpan.FromMinutes(5); // aumenta o timeout do HttpClient

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
    }

    
    public async Task<string> GenerateImageAsync(string prompt)
    {
        try
        {
            var requestData = new
            {
                prompt,
                size = "1024x1024",
                n = 1,
                response_format = "url"
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _logger.LogInformation("🔄 Enviando prompt para OpenAI: {PromptLength} caracteres", prompt.Length);

            // Removemos o CTS de 15 segundos para usar o timeout do HttpClient (5 minutos)
            var response = await _httpClient.PostAsync("images/generations", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(responseJson);

                var imageUrl = document.RootElement
                    .GetProperty("data")[0]
                    .GetProperty("url")
                    .GetString();

                _logger.LogInformation("✅ Imagem gerada com sucesso");
                return imageUrl ?? throw new Exception("URL da imagem não encontrada na resposta");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("❌ Erro OpenAI: {StatusCode} - {ErrorContent}", response.StatusCode, errorContent);
                throw new Exception("Não foi possível gerar a imagem no momento. Tente novamente.");
            }
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("⏰ Timeout na requisição para OpenAI");
            throw new Exception("A geração da imagem está demorando mais que o esperado. Tente novamente.");
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogWarning(httpEx, "🌐 Erro de rede na comunicação com OpenAI");
            throw new Exception("Problema de conexão. Verifique sua internet e tente novamente.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "❌ Falha ao gerar imagem com OpenAI");
            throw new Exception("Não foi possível gerar a imagem no momento. Tente novamente.");
        }
    }

    public async Task<string> UploadImageAsync(byte[] imageBytes, string? fileName = "upload.png")
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            // 🔎 Detectar o tipo de arquivo dinamicamente (.jpg ou .png)
            var ext = Path.GetExtension(fileName ?? "").ToLower();
            var mime = ext == ".jpg" || ext == ".jpeg" ? "image/jpeg" : "image/png";

            // 📦 Criar conteúdo multipart para envio
            var content = new MultipartFormDataContent();
            var imageContent = new ByteArrayContent(imageBytes);
            imageContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(mime);

            content.Add(imageContent, "image", fileName);
            content.Add(new StringContent("b64_json"), "response_format");

            // 🚀 Envia para o endpoint correto (variações de imagem)
            var response = await client.PostAsync("https://api.openai.com/v1/images/variations", content);
            var json = await response.Content.ReadAsStringAsync();

            // 🔐 SEG-004 — Retornar erro genérico se a API falhar
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("❌ Erro OpenAI: {StatusCode} - {Json}", response.StatusCode, json);
                throw new Exception("Erro ao processar imagem. Tente novamente mais tarde.");
            }

            using var doc = JsonDocument.Parse(json);
            var dataElement = doc.RootElement.GetProperty("data")[0];

            string? base64 = null;

            // 📤 Tenta pegar Base64, se não tiver, pega URL
            if (dataElement.TryGetProperty("b64_json", out var base64Prop))
                base64 = base64Prop.GetString();
            else if (dataElement.TryGetProperty("url", out var urlProp))
                return urlProp.GetString()!;

            if (string.IsNullOrEmpty(base64))
                throw new Exception("Erro ao processar imagem. Tente novamente mais tarde.");

            _logger.LogInformation("✅ Upload de imagem processado com sucesso ({Mime})", mime);
            return $"data:{mime};base64,{base64}";
        }
        catch (TaskCanceledException)
        {
            _logger.LogWarning("⏰ Timeout na requisição de upload para OpenAI");
            throw new Exception("Tempo limite atingido. Tente novamente mais tarde.");
        }
        catch (Exception ex)
        {
            // 🔒 SEG-004 — Nunca expor erro técnico real
            _logger.LogError(ex, "❌ Falha ao processar upload de imagem");
            throw new Exception("Erro ao processar imagem. Tente novamente mais tarde.");
        }
    }

    public async Task<string> UploadImageAsync(byte[] imageBytes)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

        // Criar conteúdo multipart
        var content = new MultipartFormDataContent();
        var imageContent = new ByteArrayContent(imageBytes);
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        content.Add(imageContent, "image", "upload.png");

        // 🚀 Força o formato base64 para garantir retorno consistente
        content.Add(new StringContent("b64_json"), "response_format");

        var response = await client.PostAsync("https://api.openai.com/v1/images/variations", content);

        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Erro na resposta da OpenAI: {response.StatusCode} - {json}");

        using var doc = JsonDocument.Parse(json);
        var dataElement = doc.RootElement.GetProperty("data")[0];

        string? base64 = null;

        // Tenta pegar base64, se não tiver, tenta pegar url
        if (dataElement.TryGetProperty("b64_json", out var base64Prop))
            base64 = base64Prop.GetString();
        else if (dataElement.TryGetProperty("url", out var urlProp))
            return urlProp.GetString()!;

        if (string.IsNullOrEmpty(base64))
            throw new Exception("Nenhuma imagem válida retornada pela API.");

        return $"data:image/png;base64,{base64}";

    }







}