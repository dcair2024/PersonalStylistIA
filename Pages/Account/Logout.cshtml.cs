using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalStylistIA.Models;
using Microsoft.Extensions.Logging;

namespace PersonalStylistIA.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("👋 Usuário fez logout com sucesso.");

            // 🔁 Redireciona para a Home (Carol usa isso pra atualizar navbar)
            return RedirectToPage("/Index");
        }
    }
}
