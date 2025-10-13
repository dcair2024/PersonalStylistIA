using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PersonalStylistIA.Models;
using System.ComponentModel.DataAnnotations; // Usado para [Required], [EmailAddress], etc.

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

        // Usamos um InputModel para agrupar as propriedades do formulário
        // e aplicar a validação com [BindProperty] apenas no modelo de entrada.
        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
            [EmailAddress(ErrorMessage = "Por favor, insira um endereço de e-mail válido.")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "O campo Senha é obrigatório.")]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo {2} caracteres.")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        public void OnGet()
        {
            // Lógica ao carregar a página
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. Validação do Modelo (formato)
            // Se algum atributo [Required], [EmailAddress], etc. falhar, retorna a página com os erros.
            if (!ModelState.IsValid)
                return Page();

            // 2. Busca o usuário pelo e-mail
            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user == null)
            {
                // Usuário não encontrado, adiciona erro genérico por segurança
                ModelState.AddModelError(string.Empty, "E-mail ou senha incorretos.");
                _logger.LogWarning("❌ Tentativa de login com e-mail não cadastrado: {Email}", Input.Email);
                return Page();
            }

            // 3. Tenta autenticar o usuário
            // Usamos o UserName que o Identity espera para PasswordSignInAsync
            var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("✅ Usuário {Email} autenticado com sucesso.", Input.Email);
                return RedirectToPage("/Index");
            }

            // 4. Se a autenticação falhou (Senha incorreta)
            _logger.LogWarning("❌ Tentativa de login inválida para {Email}", Input.Email);
            ModelState.AddModelError(string.Empty, "E-mail ou senha incorretos.");
            return Page();
        }
    }
}
