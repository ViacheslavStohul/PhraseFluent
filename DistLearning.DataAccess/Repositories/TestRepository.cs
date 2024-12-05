using Microsoft.EntityFrameworkCore;
using DistLearning.DataAccess.Entities;
using DistLearning.DataAccess.Helpers;
using DistLearning.DataAccess.Repositories.Interfaces;

namespace DistLearning.DataAccess.Repositories;

public class TestRepository(DataContext dataContext) : BaseRepository(dataContext), ITestRepository
{
    private readonly DataContext _dataContext = dataContext;
    
    public async Task<PaginationHelper<Test>> GetTestList(int page, int size, string? title)
    {
        var toSkip = SkipSize(page, size);
        
        var query = _dataContext.Tests.AsQueryable();
    
        if (!string.IsNullOrWhiteSpace(title)) 
        {
            query = query.Where(t => t.NormalizedTitle.Contains(title));
        }

        var totalItems = query.Count();

        if (totalItems == 0)
        {
            return new PaginationHelper<Test> { Items = Enumerable.Empty<Test>(), TotalItems = totalItems };
        }
        
        var items = await query
            .Skip(toSkip)
            .Take(size)
            .Include(x => x.CreatedBy)
            .AsNoTracking()
            .ToListAsync();
        
        return new PaginationHelper<Test> { Items = items, TotalItems = totalItems };
    }

    public Task<Test?> TestWithCards(Guid testUuid)
    {
        return _dataContext.Tests
            .Include(x => x.Cards)
            .ThenInclude(card => card.AnswerOptions)
            .FirstOrDefaultAsync(x => x.Uuid == testUuid);
    }

    public Task<Card?> GetCardWithOptionsByUuid(Guid cardUuid)
    {
        return _dataContext.Cards
            .Include(x => x.AnswerOptions)
            .FirstOrDefaultAsync(x => x.Uuid == cardUuid);
    }
    
    public Task<Card?> GetCardWithOptionsById(long cardId)
    {
        return _dataContext.Cards
            .Include(x => x.AnswerOptions)
            .FirstOrDefaultAsync(x => x.Id == cardId);
    }
    
    public Task<List<AnswerAttempt>> GetAnswerAttemptsForTest(long testId, Guid? answerOptionUuid)
    {
        var data = _dataContext
            .AnswerAttempts
            .Include(x => x.AnswerOption)
            .Include(x => x.Card)
            .Include(x => x.TestAttempt)
            .Where(x => x.TestAttempt.TestId == testId && x.TestAttempt.Completed);

        if (answerOptionUuid != null)
        {
            data = data.Where(x => x.TestAttempt.AnswerAttempts
                .Any(a => a.AnswerOption!.Uuid == answerOptionUuid));
        }

        return data.ToListAsync();
    }
}