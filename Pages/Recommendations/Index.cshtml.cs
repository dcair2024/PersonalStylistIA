using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PersonalStylistIA.Services;

namespace PersonalStylistIA.Pages.Recommendations
{
    public class IndexModel : PageModel
    {
        private readonly IOpenAITextService _openAITextService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IOpenAITextService openAITextService, ILogger<IndexModel> logger)
        {
            _openAITextService = openAITextService;
            _logger = logger;
        }

        // Bind do textarea do form
        [BindProperty]
        public string? UserPrompt { get; set; }

        // Estado de loading para o botão / UI
        public bool IsLoading { get; set; } = false;

        // Estado de erro (para exibir banner)
        public bool HasError { get; set; } = false;

        // Mensagem de erro detalhada (amigável)
        public string? ErrorMessage { get; set; }

        // Resultado da recomendação
        public string? Recommendation { get; set; }

        public void OnGet()
        {
            // Inicializa estados
            IsLoading = false;
            HasError = false;
            ErrorMessage = null;
            Recommendation = null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Resetar estados
            IsLoading = false;
            HasError = false;
            ErrorMessage = null;
            Recommendation = null;

            if (string.IsNullOrWhiteSpace(UserPrompt))
            {
                ModelState.AddModelError(nameof(UserPrompt), "Descreva o que você precisa.");
                return Page();
            }

            try
            {
                IsLoading = true;

                // Chama o serviço da OpenAI
                var result = await _openAITextService.GenerateTextRecommendation(UserPrompt.Trim());

                Recommendation = result?.Trim();
                IsLoading = false;

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar recomendação IA para prompt: {Prompt}", UserPrompt);
                HasError = true;
                ErrorMessage = "Não foi possível gerar a recomendação no momento. Tente novamente mais tarde.";
                IsLoading = false;
                return Page();
            }
        }
    }
}