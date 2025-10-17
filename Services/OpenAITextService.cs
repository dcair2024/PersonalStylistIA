using System.Text;
using System.Text.Json;

namespace PersonalStylistIA.Services
{
    public class OpenAITextService : IOpenAITextService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAITextService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["OpenAI:ApiKey"] ?? throw new Exception("Chave da OpenAI não configurada no appsettings.");
        }


       

        public async Task<string> GenerateTextRecommendation(string prompt)
        {
            // ✅ Cria o corpo da requisição no formato aceito pelo endpoint GPT
            var requestData = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "Você é um consultor de moda experiente. Dê sugestões criativas, práticas e com tom amigável." },
                    new { role = "user", content = prompt }
                },
                max_tokens = 250,
                temperature = 0.8
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // ✅ Autenticação da requisição
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var resultJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro da OpenAI: {response.StatusCode} - {resultJson}");

            using var doc = JsonDocument.Parse(resultJson);
            var message = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return message?.Trim() ?? "Não foi possível gerar uma recomendação no momento.";
        }
    }
}
