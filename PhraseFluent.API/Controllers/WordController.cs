using Microsoft.AspNetCore.Mvc;
using PhraseFluent.Service;
using PhraseFluent.Service.DTO.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace PhraseFluent.API.Controllers;

[Route("/word")]
public class WordController(ILogger<WordController> logger, ITranslationService translationService) : BaseController
{
    [HttpGet]
    [Route("/languages")]
    [SwaggerResponse(200, Description = "Gets all available translation languages")]
    [SwaggerResponse(500, Description = "Error when getting languages")]
    [Produces<IEnumerable<SupportedLanguage>>]
    public async Task<IActionResult> GetLanguages()
    {
        try
        {
            var languages = await translationService.GetLanguages();

            return Ok(languages);
        }
        catch (Exception ex)
        {
            logger.LogError("Error during getting languages: {Exception}", ex.Message);

            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("/translate/{wordToTranslate}/{to}")]
    [SwaggerResponse(200, Description = "Returns translation of the word")]
    [SwaggerResponse(500, Description = "Error while translation")]
    [Produces<TranslationResult>]
    public async Task<IActionResult> GetTranslation([FromRoute] string wordToTranslate, [FromRoute] string to)
    {
        try
        {
            var translation = await translationService.GetWordTranslation(wordToTranslate, to);
            
            return Ok(translation);
        }
        catch (Exception ex)
        {
            logger.LogError("Error during getting languages: {Exception}", ex.Message);

            return BadRequest();
        }
    }
    
}