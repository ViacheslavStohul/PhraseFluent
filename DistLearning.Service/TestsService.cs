using DistLearning.DataAccess.Entities;
using DistLearning.DataAccess.Enums;
using DistLearning.DataAccess.Repositories.Interfaces;
using DistLearning.Service.DTO.Requests;
using DistLearning.Service.DTO.Responses;
using DistLearning.Service.Exceptions;
using DistLearning.Service.Interfaces;
using IMapper = AutoMapper.IMapper;

namespace DistLearning.Service;

public class TestsService(ITestRepository testRepository, IMapper mapper) : ITestsService
{
    public async Task<PaginationResponse<TestResponse>> GetTestList(TestSearchRequest request)
    {
        var items = await testRepository.GetTestList(request.Page, request.Size, request.Title);

        var responses = mapper.Map<PaginationResponse<TestResponse>>(items);

        return responses;
    }

    public async Task<TestResponse> AddTest(AddTestRequest request, Guid userUuid)
    {
        var user = testRepository.GetByUuid<User>(userUuid);
        
        ArgumentNullException.ThrowIfNull(user);

        var testToAdd = new Test
        {
            Uuid = Guid.NewGuid(),
            Title = request.Title,
            NormalizedTitle = request.Title.ToUpper(),
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            UserId = user.Id,
            CardsCount = 0,
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

        switch (request.QuestionType)
        {
            case QuestionType.Text when request.AnswerOptions.Count > 1:
                throw new ArgumentException("Text questions must have only 1 answer option");
            case QuestionType.None:
                throw new ArgumentException("Invalid question type");
            case QuestionType.TestManyAnswers or QuestionType.TestOneAnswer when request.AnswerOptions.Count < 2:
                throw new ArgumentException("Для цього типу запитання виберіть хочаб дві відповіді");
        }

        await using var transaction = await testRepository.BeginTransactionAsync();
        try
        {
            cardToAdd.AnswerOptions = new List<AnswerOption>();
            testRepository.Add(cardToAdd);

            test.CardsCount += 1;

            foreach (var option in request.AnswerOptions)
            {
                var answerOption = new AnswerOption()
                {
                    Uuid = Guid.NewGuid(),
                    OptionText = option.OptionText,
                    IsAllowedText = option.IsAllowedText,
                    CardId = cardToAdd.Id
                };
                cardToAdd.AnswerOptions.Add(answerOption);
                testRepository.Add(answerOption);
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

    public async Task<TestCardResponse> BeginTestAsync(Guid testUuid)
    {
        var testWithCards = await testRepository.TestWithCards(testUuid);
        
        ArgumentNullException.ThrowIfNull(testWithCards);
        ArgumentNullException.ThrowIfNull(testWithCards.Cards);

        testWithCards.Cards = testWithCards.Cards.Where(x => x.IsActive == true).ToList();
        var questionOrder = string.Join(",", testWithCards.Cards.Select(c => c.Id));
        
        var testAttempt = new TestAttempt
        {
            Uuid = Guid.NewGuid(),
            TestId = testWithCards.Id,
            StartDate = DateTimeOffset.Now,
            EndDate = null,
            Test = testWithCards,
            QuestionOrder = questionOrder
        };
        
        testRepository.Add(testAttempt);

        await testRepository.SaveChangesAsync();

        var firstCard = testWithCards.Cards.First();
        
        return ProcessCardResponse(firstCard, testAttempt.Uuid, testWithCards.Cards.Count, 1);
    }

    public async Task<TestCardResponse?> ProcessAnswer(CardAnswerRequest request)
    {
        var card = await testRepository.GetCardWithOptionsByUuid(request.CardUuid);
        var testAttempt = testRepository.GetByUuid<TestAttempt>(request.TestAttemptUuid);
        ArgumentNullException.ThrowIfNull(card);
        ArgumentNullException.ThrowIfNull(testAttempt);

        if (testAttempt.Completed)
        {
            throw new ArgumentException("This test has already marked as completed");
        }

        await AddTestAttemptToDb(request, testAttempt.Id, card);
        
        await testRepository.SaveChangesAsync();
        
        var questionOrder = testAttempt.QuestionOrder.Split(',').Select(long.Parse).ToList();
        var nextQuestionIndex = questionOrder.IndexOf(card.Id);
        if (questionOrder.Count - 1 == nextQuestionIndex)
        {
            testAttempt.Completed = true;
            await testRepository.SaveChangesAsync();
            return new TestCardResponse()
            {
                Card = null,
                CurrentQuestion = nextQuestionIndex + 1,
                Questions = questionOrder.Count
            };
        }

        var nextQuestionId = questionOrder[nextQuestionIndex + 1];

        var nextQuestion = await testRepository.GetCardWithOptionsById(nextQuestionId);
        ArgumentNullException.ThrowIfNull(nextQuestion);
        
        return ProcessCardResponse(nextQuestion, testAttempt.Uuid, questionOrder.Count, nextQuestionIndex + 1);
    }

    private async Task AddTestAttemptToDb(CardAnswerRequest request, long testAttemptId, Card card)
    {
        var answerAttempt = new AnswerAttempt
        {
            Uuid = Guid.NewGuid(),
            TestAttemptId = testAttemptId,
            CardId = card.Id,
            TestAttempt = null,
            Card = null,
        };

        if (card.QuestionType != QuestionType.Text)
        {
            ArgumentNullException.ThrowIfNull(request.PickedOptions);
            foreach (var answerOption in request.PickedOptions)
            {
                var selectedOption = GetAnswerOptionIdByUuidFromCard(card, answerOption);  
                answerAttempt.AnswerOptionId = selectedOption.Id;

                if (selectedOption.IsAllowedText && !string.IsNullOrEmpty(request.AnswerString))
                {
                    answerAttempt.TextAnswer = request.AnswerString;
                }
                testRepository.Add(answerAttempt);
            }
        }
        else
        {
            answerAttempt.TextAnswer = request.AnswerString;
            testRepository.Add(answerAttempt);
        }
        
        await testRepository.SaveChangesAsync();
    }

    private AnswerOption GetAnswerOptionIdByUuidFromCard(Card card, Guid answerOptionUuid)
    {
        return card.AnswerOptions.First(x => x.Uuid == answerOptionUuid);
    }

    private TestCardResponse ProcessCardResponse(Card card, Guid testAttemptUuid, int allQuestions, int currentQuestion)
    {
        if (card.QuestionType == QuestionType.Text)
        {
            card.AnswerOptions = new List<AnswerOption>();
        }

        var cardResponse = mapper.Map<CardResponse>(card);

        return new TestCardResponse
        {
            Card = cardResponse,
            TestAttemptUuid = testAttemptUuid,
            Questions = allQuestions,
            CurrentQuestion = currentQuestion
        };
    }
}