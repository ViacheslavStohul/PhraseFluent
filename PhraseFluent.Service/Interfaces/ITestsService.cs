using PhraseFluent.Service.DTO.Requests;
using PhraseFluent.Service.DTO.Responses;

namespace PhraseFluent.Service.Interfaces;

public interface ITestsService
{
    Task<PaginationResponse<TestResponse>> GetTestList(TestSearchRequest request);

    Task<TestResponse> AddTest(AddTestRequest request, Guid userUuid);

    Task<CardResponseWitCorrectAnswer> CreateCard(Guid? userId, AddCardRequest request);

    Task<TestCardResponse> BeginTestAsync(Guid testUuid, Guid userId);

    Task<TestCardResponse?> ProcessAnswer(CardAnswerRequest request);
}