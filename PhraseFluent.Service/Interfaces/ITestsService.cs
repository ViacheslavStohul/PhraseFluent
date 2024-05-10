using PhraseFluent.Service.DTO.Requests;
using PhraseFluent.Service.DTO.Responses;

namespace PhraseFluent.Service.Interfaces;

public interface ITestsService
{
    Task<IEnumerable<TestResponse>> GetTestList(TestSearchRequest request);

    Task<TestResponse> AddTest(AddTestRequest request, Guid userUuid);
}