using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonalStylistIA.Services; // ajuste se o IOpenAITextService estiver em outro namespace

namespace PersonalStylistIA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IOpenAITextService _openAIService;
        private readonly ILogger<RecommendationsController> _logger;

        public RecommendationsController(IOpenAITextService openAIService, ILogger<RecommendationsController> logger)
        {
            _openAIService = openAIService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Generate([FromBody] RecommendationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Prompt))
                return BadRequest(new { success = false, error = "O campo de texto não pode estar vazio." });

            try
            {
                var recommendation = await _openAIService.GenerateTextRecommendation(request.Prompt);
                return Ok(new { success = true, recommendation });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar recomendação de IA.");
                return StatusCode(500, new { success = false, error = "Falha na comunicação com o serviço de IA." });
            }
        }
    }

    public class RecommendationRequest
    {
        public string Prompt { get; set; } = string.Empty;
    }
}

