using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalStylistIA.Services;

namespace PersonalStylistIA.Pages.Recommendations
{
    public class IndexModel : PageModel
    {
        private readonly IOpenAITextService _openAITextService;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string UserInput { get; set; } = string.Empty;

        [BindProperty]
        public string StyleSelected { get; set; } = "Casual";

        public string RecommendationResult { get; set; } = string.Empty;

        public bool IsLoading { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public IndexModel(IOpenAITextService openAITextService, ILogger<IndexModel> logger)
        {
            _openAITextService = openAITextService;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostGenerateAsync()
        {
            // ✅ Validação
            if (string.IsNullOrWhiteSpace(UserInput))
            {
                ModelState.AddModelError("UserInput", "Por favor, descreva a ocasião desejada.");
                HasError = true;
                return Page();
            }

            if (UserInput.Length < 10)
            {
                ModelState.AddModelError("UserInput", "Por favor, descreva a ocasião com mais detalhes (mínimo 10 caracteres).");
                HasError = true;
                return Page();
            }

            IsLoading = true;
            HasError = false;
            ErrorMessage = string.Empty;

            try
            {
                // ✅ Monta o prompt com estilo selecionado
                string prompt = $"Ocasião: {UserInput}\nEstilo preferido: {StyleSelected}\n\nDê sugestões de roupas, cores e acessórios específicos.";

                // ✅ Chama a API OpenAI de verdade
                RecommendationResult = await _openAITextService.GenerateTextRecommendation(prompt);

                _logger.LogInformation($"✅ Recomendação gerada com sucesso para: {UserInput}");
            }
            catch (HttpRequestException httpEx)
            {
                HasError = true;
                ErrorMessage = "Erro de conexão com a API OpenAI. Verifique sua conexão de internet e tente novamente.";
                _logger.LogError($"❌ Erro HTTP: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = "Erro ao gerar recomendação. Tente novamente mais tarde.";
                _logger.LogError($"❌ Erro geral: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }

            return Page();
        }
    }
}