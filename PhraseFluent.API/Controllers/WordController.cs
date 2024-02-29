
using Microsoft.AspNetCore.Mvc;
using PhraseFluent.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace PhraseFluent.API.Controllers;

[Microsoft.AspNetCore.Components.Route("/word")]
public class WordController(ILogger<WordController> logger, IWordService wordService) : BaseController
{
    [HttpGet]
    [Route("/GetTranslation/{wordToTranslate}/{translateTo}")]
    [SwaggerResponse(200, Description = "Get translation of the word")]
    [SwaggerResponse(400, Description = "Error during translation")]
    [Produces("application/json")]
    public async Task<IActionResult> GetWordTranslation([FromRoute] string wordToTranslate, string translateTo = "en")
    {
        try
        {
            var translationResult = await wordService.GetWordTranslation(wordToTranslate, translateTo);

            return Ok(translationResult);
        }
        catch (Exception ex)
        {
            logger.LogError("Error during translation: {Exception}", ex.Message);

            return BadRequest();
        }
    }
}