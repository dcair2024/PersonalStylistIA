using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PersonalStylistIA.Pages.Recommendations
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string UserInput { get; set; } = string.Empty;

        public string RecommendationResult { get; set; } = string.Empty;
        public bool IsLoading { get; set; }
        public bool HasError { get; set; }

        public void OnGet()
        {
            // Página carregada normalmente
        }

        public async Task<IActionResult> OnPostGenerateAsync()
        {
            if (string.IsNullOrWhiteSpace(UserInput))
            {
                ModelState.AddModelError("", "Por favor, descreva a ocasião desejada.");
                return Page();
            }

            IsLoading = true;
            HasError = false;

            try
            {
                // TODO: Davi - Integração com API OpenAI
                // RecommendationResult = await _openAIService.GetRecommendation(UserInput);

                // Mock temporário para testes
                RecommendationResult = "Sugestão: Vestido midi de seda azul-marinho, salto nude e brincos prateados discretos. Ideal para um jantar elegante à noite.";

                await Task.CompletedTask; // <- mantém método async válido sem warning
                IsLoading = false;
            }
            catch (Exception)
            {
                HasError = true;
                IsLoading = false;
                ModelState.AddModelError("", "Erro interno na geração de recomendação.");
            }

            return Page();
        }
    }
}