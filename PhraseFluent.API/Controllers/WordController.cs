using Azure;
using Azure.AI.Translation.Text;
using Microsoft.AspNetCore.Mvc;
using PhraseFluent.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace PhraseFluent.API.Controllers;

[Route("/word")]
public class WordController(ILogger<WordController> logger, ITranslationService translationService) : BaseController
{
    [HttpGet]
    [Route("/languages")]
    [SwaggerResponse(200, Description = "Gets all available translation languages")]
    [SwaggerResponse(500, Description = "Error when getting languages")]
    [Produces<Response<GetLanguagesResult>>]
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

            return StatusCode(500, "Error getting languages");
        }
    }
    
}