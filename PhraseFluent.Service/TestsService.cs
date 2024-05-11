using PhraseFluent.DataAccess.Entities;
using PhraseFluent.DataAccess.Repositories.Interfaces;
using PhraseFluent.Service.DTO.Requests;
using PhraseFluent.Service.DTO.Responses;
using PhraseFluent.Service.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace PhraseFluent.Service;

public class TestsService(ITestRepository testRepository, IMapper mapper) : ITestsService
{
    public async Task<TestSearchResponse> GetTestList(TestSearchRequest request)
    {
        var items = await testRepository.GetTestList(request.Page, request.Size, request.Language, request.Username, request.Title);

        var responses = mapper.Map<TestSearchResponse>(items);

        return responses;
    }

    public async Task<TestResponse> AddTest(AddTestRequest request, Guid userUuid)
    {
        var user = testRepository.GetByUuid<User>(userUuid);
        var language = testRepository.GetByUuid<Language>(request.LanguageUuid);
        
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(language);

        var testToAdd = new Test
        {
            Uuid = Guid.NewGuid(),
            Title = request.Title,
            NormalizedTitle = request.Title.ToUpper(),
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            UserId = user.Id,
            CardsCount = 0,
            LanguageId = language.Id,
        };
        
        testRepository.Add(testToAdd);

        await testRepository.SaveChangesAsync();
        
        return mapper.Map<TestResponse>(testToAdd);
    }
}