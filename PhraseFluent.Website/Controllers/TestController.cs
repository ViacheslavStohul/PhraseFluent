using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhraseFluent.Service.DTO.Requests;
using PhraseFluent.Service.DTO.Responses;
using PhraseFluent.Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PhraseFluent.API.Controllers;

[Route("/api/test")]
[Authorize]
public class TestController (ITestsService testsService) : BaseController
{
    [HttpGet]
    [AllowAnonymous]
    [Route("/list")]
    [SwaggerResponse(200, "Gets test list by filters")]
    [Produces<PaginationResponse<TestResponse>>]
    public async Task<IActionResult> GetTestList([FromQuery] TestSearchRequest request)
    {
        var tests = await testsService.GetTestList(request);
        
        return Ok(tests);
    }
    
    [HttpPost]
    [Route("/new")]
    [SwaggerResponse(201, "Adds a new test")]
    [ProducesResponseType(typeof(TestResponse), 201)]
    public async Task<IActionResult> AddTest([FromBody] AddTestRequest request)
    {
        var userUuid = UserId ?? Guid.Empty;
        var test = await testsService.AddTest(request, userUuid);

        return Created(string.Empty, test);
    }
    
    [HttpPost]
    [Route("/card/new")]
    [SwaggerResponse(201, "Adds a new card")]
    [ProducesResponseType(typeof(CardResponseWitCorrectAnswer), 201)]
    public async Task<IActionResult> AddCard([FromBody] AddCardRequest request)
    {
        var userId = UserId ?? Guid.Empty;
        var card = await testsService.CreateCard(userId, request);

        return Created(string.Empty, card);
    }
    
    [HttpPost]
    [Route("/begin")]
    [SwaggerResponse(200, "Begin new test")]
    [ProducesResponseType(typeof(TestCardResponse), 200)]
    public async Task<IActionResult> BeginTest([FromQuery] Guid testUuid)
    {
        var userId = UserId ?? Guid.Empty;
        var card = await testsService.BeginTestAsync(testUuid, userId);

        return Ok(card);
    }
    
    [HttpPost]
    [Route("/next")]
    [SwaggerResponse(200, "Process answer and receive next one")]
    [ProducesResponseType(typeof(TestCardResponse), 200)]
    public async Task<IActionResult> ProcessAnswer([FromBody] CardAnswerRequest request)
    {
        var card = await testsService.ProcessAnswer(request);

        return Ok(card);
    }
}