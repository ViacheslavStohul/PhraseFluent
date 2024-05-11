using PhraseFluent.DataAccess.Entities;
using PhraseFluent.DataAccess.Repositories.Interfaces;
using PhraseFluent.Service.DTO.Requests;
using PhraseFluent.Service.DTO.Responses;
using PhraseFluent.Service.Exceptions;
using PhraseFluent.Service.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace PhraseFluent.Service;

public class TestsService(ITestRepository testRepository, IMapper mapper) : ITestsService
{
    public async Task<PaginationResponse<TestResponse>> GetTestList(TestSearchRequest request)
    {
        var items = await testRepository.GetTestList(request.Page, request.Size, request.Language, request.Username, request.Title);

        var responses = mapper.Map<PaginationResponse<TestResponse>>(items);

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

    public async Task<CardResponse> CreateCard(Guid? userId, AddCardRequest request)
    {
        ArgumentNullException.ThrowIfNull(userId);
        var user = await testRepository.GetByUuidAsync<User>(userId.Value);
        ArgumentNullException.ThrowIfNull(user);

        var test = await testRepository.GetByUuidAsync<Test>(request.TestUuid);
        ArgumentNullException.ThrowIfNull(test);

        if (test.UserId != user.Id) throw new ForbiddenException();

        var cardToAdd = new Card
        {
            Uuid = Guid.NewGuid(),
            Question = request.Question,
            QuestionType = request.QuestionType,
            TestId = test.Id,
        };

        await using var transaction = await testRepository.BeginTransactionAsync();
        try
        {
            if (request.AnswerOptions != null)
            {
                cardToAdd.AnswerOptions = new List<AnswerOption>();

                foreach (var option in request.AnswerOptions)
                {
                    cardToAdd.AnswerOptions.Add(new AnswerOption
                    {
                        Uuid = Guid.NewGuid(), OptionText = option.OptionText, IsCorrect = option.IsCorrect,
                    });

                    testRepository.Add(cardToAdd);
                }
            }

            testRepository.Add(cardToAdd);
            await testRepository.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return mapper.Map<CardResponse>(cardToAdd);
    }
}