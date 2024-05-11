using PhraseFluent.DataAccess.Entities;
using PhraseFluent.DataAccess.Enums;
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
        ArgumentNullException.ThrowIfNull(request.AnswerOptions);
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

        if (request.AnswerOptions.All(x => x.IsCorrect != true))
        {
            throw new ArgumentException("There must be at least 1 correct answer");
        }

        if (request.AnswerOptions.Count > 10)
        {
            throw new ArgumentException("Cannot have more than 10 answers");
        }

        switch (request.QuestionType)
        {
            case QuestionType.Text when request.AnswerOptions.Count > 1:
                throw new ArgumentException("Text questions must have only 1 answer option");
            case QuestionType.None:
                throw new Exception("Invalid question type");
            case QuestionType.TestOneAnswer when request.AnswerOptions.Count(x => x.IsCorrect) > 1:
                throw new ArgumentException("Only one correct answer allowed in this question type");
        }

        await using var transaction = await testRepository.BeginTransactionAsync();
        try
        {
            cardToAdd.AnswerOptions = new List<AnswerOption>();
            testRepository.Add(cardToAdd);

            foreach (var option in request.AnswerOptions)
            {
                cardToAdd.AnswerOptions.Add(new AnswerOption
                {
                    Uuid = Guid.NewGuid(), OptionText = option.OptionText, IsCorrect = option.IsCorrect, CardId = cardToAdd.Id
                });

                testRepository.Add(cardToAdd);
            }
            
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