using System; // Adicione o using System para usar System.Exception
using System.Threading.Tasks;

namespace PersonalStylistIA.Services
{
    // Verifique se o nome da classe é exatamente o que está registrado no Program.cs
    public class MockOpenAITextService1 : IOpenAITextService
    {
        public Task<string> GenerateTextRecommendation(string prompt)
        {
            // CÓDIGO TEMPORÁRIO PARA O QA-027.3 - Inserir a Falha
            if (prompt.Contains("ERRO_SIMULADO_500"))
            {
                // Força o sistema a cair no bloco 'catch' do Page Model ou Controller.
                // Isso deve acionar o código SEG-004 (erro genérico).
                throw new System.Exception("Erro de conexão simulado: Timeout ou Falha de Chave.");
            }
            // FIM DO CÓDIGO TEMPORÁRIO

            // Comportamento normal do Mock se não houver erro
            var respostaMock = $"[Mock] Sugestão de estilo baseada no prompt: '{prompt}'";
            return Task.FromResult(respostaMock);
        }
    }
}