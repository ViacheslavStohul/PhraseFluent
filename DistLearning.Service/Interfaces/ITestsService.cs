using DistLearning.Service.DTO.Requests;
using DistLearning.Service.DTO.Responses;

namespace DistLearning.Service.Interfaces;

public interface ITestsService
{
    Task<PaginationResponse<TestResponse>> GetTestList(TestSearchRequest request);

    Task<TestResponse> AddTest(AddTestRequest request, Guid userUuid);

    Task<CardResponse> CreateCard(Guid? userId, AddCardRequest request);

    Task<TestCardResponse> BeginTestAsync(Guid testUuid);

    Task<TestCardResponse?> ProcessAnswer(CardAnswerRequest request);

    Task<TestWithStatisticResponse> GetTestWithStatisticsAsync(Guid testUuid);

    Task<TestWithCardsResponse> GetTestInfo(Guid testUuid);
}