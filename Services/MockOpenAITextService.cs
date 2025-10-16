using PersonalStylistIA.Services;
using System.Threading.Tasks;

namespace PersonalStylistIA.Services
{
    public class MockOpenAITextService : IOpenAITextService
    {
        public Task<string> GenerateTextRecommendation(string prompt)
        {
            // Aqui você pode simular uma resposta do GPT
            var respostaMock = $"[Mock] Sugestão de estilo baseada no prompt: '{prompt}'";
            return Task.FromResult(respostaMock);
        }
    }
}
