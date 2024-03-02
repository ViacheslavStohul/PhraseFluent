using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhraseFluent.Service;
using PhraseFluent.Service.DTO.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace PhraseFluent.API.Controllers;

[Authorize]
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

    [HttpGet]
    [Route("/examples/{sourceWord}/{targetWord}/{sourceLanguage}/{targetLanguage}")]
    [SwaggerResponse(200, Description = "Returns examples of the word in given language")]
    [SwaggerResponse(400, Description = "Error getting examples")]
    [Produces<IEnumerable<UsageExamplesResponse>>]
    public async Task<IActionResult> GetExamples([FromRoute] string sourceWord, [FromRoute] string targetWord, [FromRoute] string sourceLanguage, [FromRoute] string targetLanguage)
    {
        try
        {
            var examples = await translationService.GetExamples(sourceWord, targetWord, sourceLanguage, targetLanguage);

            return Ok(examples);
        }
        catch (Exception ex)
        {
            logger.LogError("Error getting word examples: {Exception}", ex.Message);

            return BadRequest();
        }
    }
}