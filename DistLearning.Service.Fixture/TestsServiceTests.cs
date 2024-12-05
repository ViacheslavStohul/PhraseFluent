namespace DistLeatning.Service.Fixture;

using AutoMapper;
using DistLearning.DataAccess.Entities;
using DistLearning.DataAccess.Enums;
using DistLearning.DataAccess.Repositories.Interfaces;
using DistLearning.Service;
using DistLearning.Service.DTO.Requests;
using DistLearning.Service.DTO.Responses;
using Moq;

public class TestsServiceTests
{
    private readonly Mock<ITestRepository> _testRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly TestsService _service;

    public TestsServiceTests()
    {
        _testRepositoryMock = new Mock<ITestRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new TestsService(_testRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task ProcessAnswer_ShouldTrimAnswerString_WhenAnswerStringHasLeadingAndTrailingSpaces()
    {
        var cardUuid = Guid.NewGuid();
        var testAttemptUuid = Guid.NewGuid();
        const long cardId = 1L;
        const long testAttemptId = 1L;

        var card = new Card
        {
            Id = cardId,
            Uuid = cardUuid,
            QuestionType = QuestionType.Text,
            AnswerOptions = new List<AnswerOption>(),
            Question = string.Empty
        };

        var testAttempt = new TestAttempt
        {
            Id = testAttemptId,
            Uuid = testAttemptUuid,
            Completed = false,
            QuestionOrder = cardId.ToString()
        };

        _testRepositoryMock.Setup(repo => repo.GetCardWithOptionsByUuid(cardUuid))
            .ReturnsAsync(card);

        _testRepositoryMock.Setup(repo => repo.GetByUuid<TestAttempt>(testAttemptUuid))
            .Returns(testAttempt);

        var request = new CardAnswerRequest
        {
            CardUuid = cardUuid,
            TestAttemptUuid = testAttemptUuid,
            AnswerString = "   answer with spaces   "
        };

        var addedAnswerAttempts = new List<AnswerAttempt>();

        _testRepositoryMock.Setup(repo => repo.Add(It.IsAny<AnswerAttempt>()))
            .Callback<AnswerAttempt>(aa => addedAnswerAttempts.Add(aa));

        _testRepositoryMock.Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(1);

        await _service.ProcessAnswer(request);

        Assert.Single(addedAnswerAttempts);
        var answerAttempt = addedAnswerAttempts.First();
        Assert.Equal("answer with spaces", answerAttempt.TextAnswer);
    }

    [Fact]
    public async Task ProcessAnswer_ShouldSetTextAnswerToNull_WhenAnswerStringIsEmpty()
    {
        var cardUuid = Guid.NewGuid();
        var testAttemptUuid = Guid.NewGuid();
        var cardId = 1L;
        var testAttemptId = 1L;

        var card = new Card
        {
            Id = cardId,
            Uuid = cardUuid,
            QuestionType = QuestionType.Text,
            AnswerOptions = new List<AnswerOption>(),
            Question = string.Empty
        };

        var testAttempt = new TestAttempt
        {
            Id = testAttemptId,
            Uuid = testAttemptUuid,
            Completed = false,
            QuestionOrder = cardId.ToString()
        };

        _testRepositoryMock.Setup(repo => repo.GetCardWithOptionsByUuid(cardUuid))
            .ReturnsAsync(card);

        _testRepositoryMock.Setup(repo => repo.GetByUuid<TestAttempt>(testAttemptUuid))
            .Returns(testAttempt);

        var request = new CardAnswerRequest
        {
            CardUuid = cardUuid,
            TestAttemptUuid = testAttemptUuid,
            AnswerString = ""
        };

        var addedAnswerAttempts = new List<AnswerAttempt>();

        _testRepositoryMock.Setup(repo => repo.Add(It.IsAny<AnswerAttempt>()))
            .Callback<AnswerAttempt>(aa => addedAnswerAttempts.Add(aa));

        _testRepositoryMock.Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(1);

        await _service.ProcessAnswer(request);

        Assert.Single(addedAnswerAttempts);
        var answerAttempt = addedAnswerAttempts.First();
        Assert.Null(answerAttempt.TextAnswer);
    }

    [Fact]
    public async Task ProcessAnswer_ShouldThrowArgumentException_WhenTestAttemptIsCompleted()
    {
        var cardUuid = Guid.NewGuid();
        var testAttemptUuid = Guid.NewGuid();

        var card = new Card { Uuid = cardUuid, Question = string.Empty };
        var testAttempt = new TestAttempt { Uuid = testAttemptUuid, Completed = true, QuestionOrder = "1,2,3"};

        _testRepositoryMock.Setup(repo => repo.GetCardWithOptionsByUuid(cardUuid))
            .ReturnsAsync(card);

        _testRepositoryMock.Setup(repo => repo.GetByUuid<TestAttempt>(testAttemptUuid))
            .Returns(testAttempt);

        var request = new CardAnswerRequest
        {
            CardUuid = cardUuid,
            TestAttemptUuid = testAttemptUuid,
            AnswerString = null
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.ProcessAnswer(request));
    }

    [Fact]
    public async Task ProcessAnswer_ShouldProcessNextQuestion_WhenNotLastQuestion()
    {
        // Arrange
        var cardUuid = Guid.NewGuid();
        var testAttemptUuid = Guid.NewGuid();
        var cardId = 1L;
        var nextCardId = 2L;
        var testAttemptId = 1L;

        var card = new Card
        {
            Id = cardId,
            Uuid = cardUuid,
            QuestionType = QuestionType.Text,
            AnswerOptions = new List<AnswerOption>(),
            Question = string.Empty
        };

        var nextCard = new Card
        {
            Id = nextCardId,
            Uuid = Guid.NewGuid(),
            QuestionType = QuestionType.Text,
            AnswerOptions = new List<AnswerOption>(),
            Question = string.Empty
        };

        var testAttempt = new TestAttempt
        {
            Id = testAttemptId,
            Uuid = testAttemptUuid,
            Completed = false,
            QuestionOrder = $"{cardId},{nextCardId}"
        };

        _testRepositoryMock.Setup(repo => repo.GetCardWithOptionsByUuid(cardUuid))
            .ReturnsAsync(card);

        _testRepositoryMock.Setup(repo => repo.GetByUuid<TestAttempt>(testAttemptUuid))
            .Returns(testAttempt);

        _testRepositoryMock.Setup(repo => repo.GetCardWithOptionsById(nextCardId))
            .ReturnsAsync(nextCard);

        _mapperMock.Setup(m => m.Map<CardResponse>(It.IsAny<Card>()))
            .Returns(new CardResponse
            {
                Question =  string.Empty
            });

        var request = new CardAnswerRequest
        {
            CardUuid = cardUuid,
            TestAttemptUuid = testAttemptUuid,
            AnswerString = "   next question   "
        };

        var expectedResult = request.AnswerString.TrimEnd();
        expectedResult = expectedResult.TrimStart();

        var addedAnswerAttempts = new List<AnswerAttempt>();

        _testRepositoryMock.Setup(repo => repo.Add(It.IsAny<AnswerAttempt>()))
            .Callback<AnswerAttempt>(aa => addedAnswerAttempts.Add(aa));

        _testRepositoryMock.Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(1);

        var response = await _service.ProcessAnswer(request);
        
        Assert.NotNull(response);
        Assert.NotNull(response.Card);
        Assert.Equal(2, response.Questions);
        Assert.Equal(2, response.CurrentQuestion);
    }
}