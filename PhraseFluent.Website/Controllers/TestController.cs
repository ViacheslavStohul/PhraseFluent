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

        return Created("", test);
    }
    
    [HttpPost]
    [Route("/card/new")]
    [SwaggerResponse(201, "Adds a new card")]
    [ProducesResponseType(typeof(CardResponse), 201)]
    public async Task<IActionResult> AddCard([FromBody] AddCardRequest request)
    {
        var userId = UserId;
        var card = await testsService.CreateCard(userId, request);

        return Created("", card);
    }
}