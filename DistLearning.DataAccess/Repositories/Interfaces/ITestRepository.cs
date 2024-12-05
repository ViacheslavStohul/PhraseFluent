using DistLearning.DataAccess.Entities;
using DistLearning.DataAccess.Helpers;

namespace DistLearning.DataAccess.Repositories.Interfaces;

public interface ITestRepository : IBaseRepository
{
    public Task<PaginationHelper<Test>> GetTestList(int page, int size, string? title);

    Task<Test?> TestWithCards(Guid testUuid);

    Task<Card?> GetCardWithOptionsByUuid(Guid cardUuid);

    Task<Card?> GetCardWithOptionsById(long cardId);

    Task<List<AnswerAttempt>> GetAnswerAttemptsForTest(long testId, Guid? answerOptionUuid);
}