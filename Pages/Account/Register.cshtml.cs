using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalStylistIA.Models;
using Microsoft.Extensions.Logging;

namespace PersonalStylistIA.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public string Nome { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("🟡 INICIANDO CADASTRO - Nome: {Nome}, Email: {Email}", Nome, Email);

            // 1. ADIÇÃO: Verifica a validação automática do ModelState
            if (!ModelState.IsValid)
            {
                // Esta linha captura erros de DataAnnotations (ex: formato de email incorreto)
                _logger.LogError("❌ ModelState inválido na submissão do formulário.");
                return Page();
            }

            // 2. VERIFICAÇÃO DE SENHAS: Esta lógica está correta
            if (Password != ConfirmPassword)
            {
                _logger.LogWarning("❌ SENHAS NÃO COINCIDEM");
                ModelState.AddModelError("ConfirmPassword", "As senhas não coincidem");
                return Page();
            }

            // A verificação de string.IsNullOrEmpty é redundante se você usar [Required]
            // no PageModel, mas como não o vimos, a lógica atual funciona, embora seja manual.

            try
            {
                // 3. VERIFICAÇÃO DE EMAIL EXISTENTE: Esta lógica está correta
                var existingUser = await _userManager.FindByEmailAsync(Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("❌ EMAIL JÁ EXISTE: {Email}", Email);
                    ModelState.AddModelError("Email", "Este email já está cadastrado");
                    return Page();
                }

                var user = new ApplicationUser
                {
                    UserName = Email,
                    Email = Email,
                    NomeCompleto = Nome
                };

                _logger.LogInformation("🟡 CRIANDO USUÁRIO NO BANCO...");
                var result = await _userManager.CreateAsync(user, Password);

                // 4. VERIFICAÇÃO DO RESULTADO DO IDENTITY: Esta lógica está correta e completa
                if (result.Succeeded)
                {
                    _logger.LogInformation("✅ USUÁRIO CRIADO COM SUCESSO: {Email}", Email);

                    
                    return RedirectToPage("/Account/Login");

                    
                }
                else
                {
                    _logger.LogError("❌ ERRO IDENTITY: {Errors}",
                        string.Join(", ", result.Errors.Select(e => e.Description)));

                    foreach (var error in result.Errors)
                    {
                        // Adiciona erros como senha fraca ao ModelState
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ ERRO EXCEÇÃO AO CRIAR USUÁRIO");
                ModelState.AddModelError(string.Empty, "Erro interno ao criar conta");
            }

            return Page();
        }
    }
}