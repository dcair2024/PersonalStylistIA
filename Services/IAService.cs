using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PersonalStylistIA.Services
{
    public class IAService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public IAService(IConfiguration config)
        {
            _apiKey = config["OpenAI:ApiKey"];
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        // Método pra gerar imagem a partir de um prompt de texto
        public async Task<byte[]> GerarImagemAsync(string prompt)
        {
            var body = new
            {
                model = "gpt-image-1",
                prompt = prompt,
                size = "512x512"
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/images/generations", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var base64 = doc.RootElement.GetProperty("data")[0].GetProperty("b64_json").GetString();

            return Convert.FromBase64String(base64);
        }

        // Método pra fazer upload de imagem (futuramente usar image edit/variation)
        public async Task<byte[]> UploadImagemAsync(byte[] imagemBytes)
        {
            var form = new MultipartFormDataContent();
            form.Add(new ByteArrayContent(imagemBytes), "image", "upload.png");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/images/edits", form);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var base64 = doc.RootElement.GetProperty("data")[0].GetProperty("b64_json").GetString();

            return Convert.FromBase64String(base64);
        }
    }
}
