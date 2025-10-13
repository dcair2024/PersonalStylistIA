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

        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session?.Clear(); // ✅ usa null-conditional
            _logger.LogInformation("🟢 Sessão finalizada com sucesso.");
            return RedirectToPage("/Index");
            _logger.LogInformation("🧹 Usuário desconectado com sucesso.");
            return RedirectToPage("/Index");     // ✅ redireciona pra home
        }
    }
    
}
