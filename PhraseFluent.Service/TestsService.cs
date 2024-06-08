﻿using PhraseFluent.DataAccess.Entities;
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

    public async Task<CardResponseWitCorrectAnswer> CreateCard(Guid? userId, AddCardRequest request)
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
            case QuestionType.TestManyAnswers or QuestionType.TestOneAnswer when request.AnswerOptions.Count < 2:
                throw new ArgumentException("This type of question must have least 2 answer options");
        }

        await using var transaction = await testRepository.BeginTransactionAsync();
        try
        {
            cardToAdd.AnswerOptions = new List<AnswerOption>();
            testRepository.Add(cardToAdd);

            test.CardsCount += 1;

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

        return mapper.Map<CardResponseWitCorrectAnswer>(cardToAdd);
    }

    public async Task<TestCardResponse> BeginTestAsync(Guid testUuid, Guid userId)
    {
        var testWithCards = await testRepository.TestWithCards(testUuid);
        var user = testRepository.GetByUuid<User>(userId) ?? throw new ForbiddenException();
        
        ArgumentNullException.ThrowIfNull(testWithCards);
        ArgumentNullException.ThrowIfNull(testWithCards.Cards);

        testWithCards.Cards = testWithCards.Cards.Where(x => x.IsActive == true).ToList();
        var shuffledCards = testWithCards.Cards.OrderBy(c => Guid.NewGuid()).ToList();
        var questionOrder = string.Join(",", shuffledCards.Select(c => c.Id));
        
        var testAttempt = new TestAttempt
        {
            Uuid = Guid.NewGuid(),
            TestId = testWithCards.Id,
            UserId = user.Id,
            CorrectAnswers = 0,
            WrongAnswers = 0,
            PartiallyCorrectAnswers = 0,
            OverallResult = 0,
            StartDate = DateTimeOffset.Now,
            EndDate = null,
            Test = testWithCards,
            User = user,
            QuestionOrder = questionOrder
        };
        
        testRepository.Add(testAttempt);

        await testRepository.SaveChangesAsync();

        var firstCard = shuffledCards[0];
        
        return ProcessCardResponse(firstCard, testAttempt.Uuid, shuffledCards.Count, 1);
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

        var answerResult = GetAnswerResult(request, card);

        await AddTestAttemptToDb(request, testAttempt.Id, card, answerResult);
        
        CalculateTestResults(answerResult, testAttempt);
        
        await testRepository.SaveChangesAsync();
        
        var questionOrder = testAttempt.QuestionOrder.Split(',').Select(long.Parse).ToList();
        var nextQuestionIndex = questionOrder.IndexOf(card.Id);
        if (questionOrder.Count == nextQuestionIndex)
        {
            testAttempt.Completed = true;
            await testRepository.SaveChangesAsync();
            return new TestCardResponse()
            {
                Card = null,
                CurrentQuestion = nextQuestionIndex - 1,
                Questions = questionOrder.Count - 1
            };
        }

        var nextQuestionId = questionOrder[nextQuestionIndex + 1];

        var nextQuestion = await testRepository.GetCardWithOptionsById(nextQuestionId);
        ArgumentNullException.ThrowIfNull(nextQuestion);
        
        return ProcessCardResponse(nextQuestion, testAttempt.Uuid, questionOrder.Count, nextQuestionIndex - 1);
    }

    private static void CalculateTestResults(AnswerResult answerResult, TestAttempt testAttempt)
    {
        switch (answerResult)
        {
            case AnswerResult.Correct:
                testAttempt.CorrectAnswers++;
                break;
            case AnswerResult.Wrong:
                testAttempt.WrongAnswers++;
                break;
            case AnswerResult.PartiallyCorrect:
                testAttempt.PartiallyCorrectAnswers++;
                break;
        }

        var totalQuestions = testAttempt.CorrectAnswers + testAttempt.WrongAnswers + testAttempt.PartiallyCorrectAnswers;
        testAttempt.OverallResult = totalQuestions > 0 ? (testAttempt.CorrectAnswers * 100) / totalQuestions : 0;
    }

    private async Task AddTestAttemptToDb(CardAnswerRequest request, long testAttemptId, Card card, AnswerResult answerResult)
    {
        var answerAttempt = new AnswerAttempt
        {
            Uuid = Guid.NewGuid(),
            TestAttemptId = testAttemptId,
            CardId = card.Id,
            AnswerResult = answerResult,
            TestAttempt = null,
            Card = null,
        };

        if (card.QuestionType != QuestionType.Text)
        {
            ArgumentNullException.ThrowIfNull(request.PickedOptions);
            foreach (var answerOption in request.PickedOptions)
            {
                answerAttempt.AnswerOptionId = GetAnswerOptionIdByUuidFromCard(card, answerOption);
                testRepository.Add(answerAttempt);
            }
        }
        else
        {
            answerAttempt.TextAnswer = request.AnswerString;
            testRepository.Add(answerAttempt);
        }
    }

    private long GetAnswerOptionIdByUuidFromCard(Card card, Guid answerOptionUuid)
    {
        return card.AnswerOptions.First(x => x.Uuid == answerOptionUuid).Id;
    }

    private AnswerResult GetAnswerResult(CardAnswerRequest answer, Card cardToAnswer)
    {
        ArgumentNullException.ThrowIfNull(cardToAnswer.AnswerOptions);

        return cardToAnswer.QuestionType switch
        {
            QuestionType.Text => EvaluateTextAnswer(answer, cardToAnswer),
            QuestionType.TestOneAnswer => EvaluateTestOneAnswer(answer, cardToAnswer),
            QuestionType.TestManyAnswers => EvaluateTestManyAnswers(answer, cardToAnswer),
            _ => AnswerResult.UnAnswered
        };
    }

    private AnswerResult EvaluateTextAnswer(CardAnswerRequest answer, Card cardToAnswer)
    {
        var correctAnswer = cardToAnswer.AnswerOptions.First().OptionText;

        if (string.IsNullOrEmpty(answer.AnswerString))
        {
            return AnswerResult.Wrong;
        }

        return string.Equals(answer.AnswerString.TrimStart(), correctAnswer, StringComparison.CurrentCultureIgnoreCase)
            ? AnswerResult.Correct
            : AnswerResult.Wrong;
    }

    private AnswerResult EvaluateTestOneAnswer(CardAnswerRequest answer, Card cardToAnswer)
    {
        ArgumentNullException.ThrowIfNull(answer.PickedOptions);

        return answer.PickedOptions.First() == cardToAnswer.AnswerOptions.First(x => x.IsCorrect).Uuid
            ? AnswerResult.Correct
            : AnswerResult.Wrong;
    }

    private AnswerResult EvaluateTestManyAnswers(CardAnswerRequest answer, Card cardToAnswer)
    {
        ArgumentNullException.ThrowIfNull(answer.PickedOptions);

        var correctAnswers = cardToAnswer.AnswerOptions.Where(x => x.IsCorrect).Select(x => x.Uuid).ToList();
        var pickedOptions = answer.PickedOptions.ToList();

        var correctAnswersCount = correctAnswers.Count(correctAnswer => pickedOptions.Contains(correctAnswer));

        return correctAnswersCount == 0
            ? AnswerResult.Wrong
            : correctAnswersCount == correctAnswers.Count && correctAnswersCount == pickedOptions.Count
                ? AnswerResult.Correct
                : AnswerResult.PartiallyCorrect;
    }


    private TestCardResponse ProcessCardResponse(Card card, Guid testAttemptUuid, int allQuestions, int currentQuestion)
    {
        if (card.QuestionType == QuestionType.Text)
        {
            card.AnswerOptions = new List<AnswerOption>();
        }

        var cardResponse = mapper.Map<BaseCardResponse>(card);

        return new TestCardResponse
        {
            Card = cardResponse,
            TestAttemptUuid = testAttemptUuid,
            Questions = allQuestions,
            CurrentQuestion = currentQuestion
        };
    }
}