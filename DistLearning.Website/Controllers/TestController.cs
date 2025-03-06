using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DistLearning.Service.DTO.Requests;
using DistLearning.Service.DTO.Responses;
using DistLearning.Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace DistLearning.API.Controllers;

using Microsoft.IdentityModel.Tokens;

[Route("/api/test")]
[Authorize]
public class TestController (ITestsService testsService) : BaseController
{
    [HttpGet]
    [AllowAnonymous]
    [Route("/list")]
    [SwaggerResponse(200, "Gets test list by filters", typeof(PaginationResponse<TestResponse>))]
    [Produces<PaginationResponse<TestResponse>>]
    public async Task<IActionResult> GetTestList([FromQuery] TestSearchRequest request)
    {
        var tests = await testsService.GetTestList(request);
        
        return Ok(tests);
    }
    
    [HttpGet]
    [SwaggerResponse(200, "Gets test", typeof(TestWithCardsResponse))]
    [AllowAnonymous]
    [Produces<TestWithCardsResponse>]
    public async Task<IActionResult> GetUserTestList(Guid uuid)
    {
        var tests = await testsService.GetTestInfo(uuid, UserId);
        
        return Ok(tests);
    }
    
    [HttpPost]
    [Route("/new")]
    [SwaggerResponse(201, "Adds a new test", typeof(TestResponse))]
    [ProducesResponseType(typeof(TestResponse), 201)]
    public async Task<IActionResult> AddTest([FromBody] AddTestRequest request)
    {
        var userUuid = UserId ?? Guid.Empty;
        var test = await testsService.AddTest(request, userUuid);

        return Created(string.Empty, test);
    }
    
    [HttpPost]
    [Route("/card/new")]
    [SwaggerResponse(201, "Adds a new card", typeof(CardResponse))]
    [ProducesResponseType(typeof(CardResponse), 201)]
    public async Task<IActionResult> AddCard([FromBody] AddCardRequest request)
    {
        var userId = UserId ?? Guid.Empty;
        var card = await testsService.CreateCard(userId, request);

        return Created(string.Empty, card);
    }
    
    [HttpPost]
    [Route("/begin")]
    [AllowAnonymous]
    [SwaggerResponse(200, "Begin new test", typeof(TestCardResponse))]
    [ProducesResponseType(typeof(TestCardResponse), 200)]
    public async Task<IActionResult> BeginTest([FromQuery] Guid testUuid)
    {
        var card = await testsService.BeginTestAsync(testUuid);

        return Ok(card);
    }
    
    [HttpPost]
    [Route("/next")]
    [AllowAnonymous]
    [SwaggerResponse(200, "Process answer and receive next one", typeof(TestCardResponse))]
    [ProducesResponseType(typeof(TestCardResponse), 200)]
    public async Task<IActionResult> ProcessAnswer([FromBody] CardAnswerRequest request)
    {
        var card = await testsService.ProcessAnswer(request);

        return Ok(card);
    }

    [HttpGet("/statistics")]
    [SwaggerResponse(200, "Gets test statistics", typeof(TestWithStatisticResponse))]
    [ProducesResponseType(typeof(TestWithStatisticResponse), 200)]
    public async Task<IActionResult> GetTestStatistics([FromQuery] Guid testUuid, [FromQuery] Guid? answerOptionUuid = null)
    {
        var statistics = await testsService.GetTestWithStatisticsAsync(testUuid, answerOptionUuid).ConfigureAwait(false);

        return Ok(statistics);
    }
    
    [HttpGet("/statistics/excel")]
    [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [SwaggerResponse(200, "Gets test statistics as an Excel file", typeof(FileContentResult))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportStatisticsToExcel([FromQuery] Guid testUuid)
    {
        var statistics = await testsService.ExportTestToExcel(testUuid).ConfigureAwait(false);

        return File(
            statistics, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            "statistics.xlsx"
        );
    }
}