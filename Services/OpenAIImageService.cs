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
        _httpClient.Timeout = TimeSpan.FromSeconds(15);

        // ✅ CONFIGURAR HEADER DE AUTORIZAÇÃO UMA VEZ
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> GenerateImageAsync(string prompt)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
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

            var response = await _httpClient.PostAsync("images/generations", content, cts.Token);

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
        catch (TaskCanceledException) when (!cts.Token.IsCancellationRequested)
        {
            _logger.LogWarning("⏰ Timeout na requisição para OpenAI após 15 segundos");
            throw new Exception("A geração está demorando mais que o esperado. Tente novamente.");
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("⏰ Requisição cancelada por timeout");
            throw new Exception("Tempo limite excedido. Tente uma descrição mais simples.");
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
}