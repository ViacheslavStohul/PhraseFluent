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

    public async Task<Test?> TestWithCards(Guid testUuid)
    {
        var test = await _dataContext.Tests
            .Include(x => x.Cards)
            .ThenInclude(card => card.AnswerOptions)
            .FirstOrDefaultAsync(x => x.Uuid == testUuid);

        return test;
    }

    public async Task<Card?> GetCardWithOptionsByUuid(Guid cardUuid)
    {
        var card = await _dataContext.Cards
            .Include(x => x.AnswerOptions)
            .FirstOrDefaultAsync(x => x.Uuid == cardUuid);

        return card;
    }
    
    public async Task<Card?> GetCardWithOptionsById(long cardId)
    {
        var card = await _dataContext.Cards
            .Include(x => x.AnswerOptions)
            .FirstOrDefaultAsync(x => x.Id == cardId);

        return card;
    }
}