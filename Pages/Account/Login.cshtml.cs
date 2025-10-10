using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalStylistIA.Models;
using Microsoft.Extensions.Logging;

namespace PersonalStylistIA.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager,
                          UserManager<ApplicationUser> userManager,
                          ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public void OnGet()
        {
            // Página de login normal — sem teste de usuário automático
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError(string.Empty, "Preencha todos os campos obrigatórios.");
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                // 🔴 Mantém mensagem genérica para segurança (sem revelar se o email existe)
                ModelState.AddModelError(string.Empty, "E-mail ou senha incorretos.");
                _logger.LogWarning("❌ Tentativa de login com e-mail não cadastrado: {Email}", Email);
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("✅ Usuário {Email} autenticado com sucesso.", Email);
                return RedirectToPage("/Index");
            }

            // 🔐 Mensagem genérica para falhas de autenticação (BUG-001 resolvido)
            _logger.LogWarning("❌ Tentativa de login inválida para {Email}", Email);
            ModelState.AddModelError(string.Empty, "E-mail ou senha incorretos.");
            return Page();
        }

    }
}
