using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace PersonalStylistIA.Pages
{
    public class PromptModel : PageModel
    {
        private readonly IOpenAIImageService _imageService;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ImagemGeradaUrl { get; set; }
        public string? MensagemErro { get; set; }
        public bool ProcessamentoIniciado { get; set; }
        public bool OcorreuErro { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "O campo é obrigatório.")]
            [StringLength(200, ErrorMessage = "Máximo de 200 caracteres.")]
            public string? TextoDigitado { get; set; }
            public string? Ocasiao { get; set; }
            public string? Cores { get; set; }
        }

        public PromptModel(IOpenAIImageService imageService)
        {
            _imageService = imageService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            // Limpar estados anteriores
            MensagemErro = null;
            OcorreuErro = false;

            if (!ModelState.IsValid)
            {
                return Page(); // Validação falha, mantém o estado atual
            }

            try
            {
                ProcessamentoIniciado = true;
                if (!string.IsNullOrWhiteSpace(Input.TextoDigitado))
                {
                    // Montar prompt completo com filtros
                    var promptCompleto = Input.TextoDigitado;

                    if (!string.IsNullOrEmpty(Input.Ocasiao))
                    {
                        promptCompleto += $", ocasião: {Input.Ocasiao}";
                    }

                    if (!string.IsNullOrEmpty(Input.Cores))
                    {
                        promptCompleto += $", cores: {Input.Cores}";
                    }

                    ImagemGeradaUrl = await _imageService.GenerateImageAsync(promptCompleto);
                }
            }
            catch (Exception ex)
            {
                OcorreuErro = true;
                MensagemErro = "Não foi possível gerar a imagem. Tente novamente. Detalhe: " + ex.Message;
                ImagemGeradaUrl = GerarImagemFake(Input.TextoDigitado); // Fallback original
            }
            finally
            {
                ProcessamentoIniciado = false;
            }

            return Page();
        }

        private string GerarImagemFake(string texto)
        {
            var textoUrl = System.Net.WebUtility.UrlEncode(texto);
            return $"https://dummyimage.com/600x400/cccccc/000000&text={textoUrl}";
        }

        [BindProperty]
        public IFormFile? ImagemEnviada { get; set; }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            MensagemErro = null;
            OcorreuErro = false;

            if (ImagemEnviada == null || ImagemEnviada.Length == 0)
            {
                OcorreuErro = true;
                MensagemErro = "Nenhuma imagem foi selecionada.";
                return Page();
            }

            try
            {
                ProcessamentoIniciado = true;

                using var ms = new MemoryStream();
                await ImagemEnviada.CopyToAsync(ms);
                var imagemBytes = ms.ToArray();

                // 🔧 Usa IAService (ou OpenAIImageService) para processar o upload
                var imagemProcessada = await _imageService.UploadImageAsync(imagemBytes);

                ImagemGeradaUrl = imagemProcessada;
            }
            catch (Exception ex)
            {
                OcorreuErro = true;
                MensagemErro = "Erro ao enviar imagem para IA. " + ex.Message;
            }
            finally
            {
                ProcessamentoIniciado = false;
            }

            return Page();
        }

    }
}