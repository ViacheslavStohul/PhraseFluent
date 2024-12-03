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
    
    public async Task<TestResponse> GetTestInfo(Guid testUuid, Guid? userUuid)
    {
        if (userUuid == null)
        {
            var test = await testRepository.GetByUuidAsync<Test>(testUuid);
            var response = mapper.Map<TestResponse>(test);
            return response;
        }
        else
        {
            var test =  await testRepository.TestWithCards(testUuid);
            ArgumentNullException.ThrowIfNull(test);
            var response = mapper.Map<TestWithCardsResponse>(test);
            return response;
        }
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

    public async Task<TestWithStatisticResponse> GetTestWithStatisticsAsync(Guid testUuid)
    {
        var test = await testRepository.TestWithCards(testUuid);
        ArgumentNullException.ThrowIfNull(test);
        
        var answerAttempts = await testRepository.GetAnswerAttemptsForTest(test.Id);

        var testDto = new TestWithStatisticResponse
        {
            Uuid = test.Uuid,
            Title = test.Title,
            Description = test.Description,
            ImageUrl = test.ImageUrl,
            CardsCount = test.CardsCount,
            CompletedAttempts = answerAttempts.Count > 0 ? answerAttempts.GroupBy(x => x.TestAttemptId).Count() : 0,
            Cards = []
        };

        if (test.Cards != null && test.Cards.Count != 0)
        {
            ProcessCardStatistic(test, answerAttempts, testDto);
        }
        
        return testDto;
    }

    public async Task<CardResponse> CreateCard(Guid? userId, AddCardRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
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
        if (testWithCards.Cards == null || testWithCards.Cards.Count == 0)
        {
            throw new ArgumentException("Test has no questions");
        }

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
                CurrentQuestion = questionOrder.Count,
                Questions = questionOrder.Count
            };
        }

        var nextQuestionId = questionOrder[nextQuestionIndex + 1];

        var nextQuestion = await testRepository.GetCardWithOptionsById(nextQuestionId);
        ArgumentNullException.ThrowIfNull(nextQuestion);
        
        return ProcessCardResponse(nextQuestion, testAttempt.Uuid, questionOrder.Count, nextQuestionIndex + 2);
    }
    
    private static void ProcessCardStatistic(Test test, List<AnswerAttempt> answerAttempts, TestWithStatisticResponse testDto)
    {
        ArgumentNullException.ThrowIfNull(test.Cards);
        foreach (var card in test.Cards)
        {
            var cardDto = new CardWithStatisticsResponse
            {
                Uuid = card.Uuid,
                Question = card.Question,
                QuestionType = card.QuestionType,
                AnswerOptions = [],
                TextAnswers = []
            };
            
            var cardAnswerAttempts = answerAttempts.Where(a => a.CardId == card.Id).ToList();
            var totalAttempts = cardAnswerAttempts.GroupBy(x => x.TestAttemptId).Select(x => x.Key).Count();
            
            ProcessAnswerOptionStatistic(card, cardAnswerAttempts, totalAttempts, cardDto, testDto);
        }
    }
    
    private static void ProcessAnswerOptionStatistic(Card card, List<AnswerAttempt> cardAnswerAttempts, int totalAttempts, CardWithStatisticsResponse cardDto, TestWithStatisticResponse testDto)
    {
        var textAnswers = cardAnswerAttempts
            .Where(a => !string.IsNullOrWhiteSpace(a.TextAnswer))
            .GroupBy(a => a.TextAnswer!.ToLowerInvariant())
            .Select(g => new TextAnswerResponse()
            {
                Text = g.Key!,
                Count = g.Count()
            })
            .ToList();

        cardDto.TextAnswers = textAnswers;
        
        foreach (var answerOption in card.AnswerOptions)
        {
            var optionAttempts = cardAnswerAttempts
                .Where(a => a.AnswerOptionId == answerOption.Id)
                .ToList();

            var selectionCount = optionAttempts.Count;
            var percentage = totalAttempts > 0 ? (double)selectionCount / totalAttempts * 100 : 0;

            var answerOptionDto = new AnswerResponseWithStatistics()
            {
                Uuid = answerOption.Uuid,
                OptionText = answerOption.OptionText,
                SelectionCount = selectionCount,
                SelectionPercentage = percentage,
                IsAllowedText = answerOption.IsAllowedText,
            };

            cardDto.AnswerOptions.Add(answerOptionDto);
        }
        
        testDto.Cards.Add(cardDto);
    }

    private async Task AddTestAttemptToDb(CardAnswerRequest request, long testAttemptId, Card card)
    {
        if (card.QuestionType != QuestionType.Text)
        {
            ArgumentNullException.ThrowIfNull(request.PickedOptions);
            foreach (var answerOption in request.PickedOptions)
            {
                var answerAttempt = new AnswerAttempt
                {
                    Uuid = Guid.NewGuid(),
                    TestAttemptId = testAttemptId,
                    CardId = card.Id,
                    TestAttempt = null,
                    Card = null,
                };
                
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
            var answerAttempt = new AnswerAttempt
            {
                Uuid = Guid.NewGuid(),
                TestAttemptId = testAttemptId,
                CardId = card.Id,
                TestAttempt = null,
                Card = null,
                TextAnswer = request.AnswerString
            };

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