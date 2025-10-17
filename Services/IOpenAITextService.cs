using System.Threading.Tasks;

namespace PersonalStylistIA.Services
{
    public interface IOpenAITextService
    {
        /// <summary>
        /// Gera uma recomendação textual de moda baseada em um prompt do usuário.
        /// </summary>
        /// <param name="prompt">Texto de entrada do usuário (descrição, dúvida ou estilo desejado).</param>
        /// <returns>Uma string com a resposta gerada pela IA.</returns>
        Task<string> GenerateTextRecommendation(string prompt);
    }
}
