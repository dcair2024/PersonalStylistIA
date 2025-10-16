using System.Threading.Tasks;

namespace PersonalStylistIA.Services
{
    public class MockOpenAITextService1 : IOpenAITextService
    {
        public Task<string> GenerateTextRecommendation(string prompt)
        {
            // CÓDIGO TEMPORÁRIO PARA O QA-027.3
            if (prompt.Contains("ERRO_SIMULADO_500"))
            {
                // Força o sistema a cair no bloco 'catch' do Page Model.
                throw new System.Exception("Erro de conexão simulado: Timeout ou Falha de Chave.");
            }
            // FIM DO CÓDIGO TEMPORÁRIO

            // Comportamento normal do Mock se não houver erro
            var respostaMock = $"[Mock] Sugestão de estilo baseada no prompt: '{prompt}'";
            return Task.FromResult(respostaMock);
        }
    }
}