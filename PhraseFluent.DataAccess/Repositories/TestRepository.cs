using Microsoft.EntityFrameworkCore;
using PhraseFluent.DataAccess.Entities;
using PhraseFluent.DataAccess.Helpers;
using PhraseFluent.DataAccess.Repositories.Interfaces;

namespace PhraseFluent.DataAccess.Repositories;

public class TestRepository(DataContext dataContext) : BaseRepository(dataContext), ITestRepository
{
    private readonly DataContext _dataContext = dataContext;
    
    public async Task<PaginationHelper<Test>> GetTestList(int page, int size, string? language, string? username, string? title)
    {
        var toSkip = SkipSize(page, size);
        
        var query = _dataContext.Tests.AsQueryable();

        if (!string.IsNullOrWhiteSpace(language))
        {
            query = query
                .Join(_dataContext.Languages, test => test.LanguageId, lang => lang.Id,
                    (test, lang) => new { Test = test, Language = lang }).Where(joined =>
                    joined.Language.LanguageCode == language || joined.Language.NormalizedTitle.Contains(language) ||
                    joined.Language.NormalizedNativeName.Contains(language))
                .Select(joined => joined.Test); 
        }
    
        if (!string.IsNullOrWhiteSpace(username)) 
        {
            query = query.Join(_dataContext.Users, test => test.UserId, user => user.Id, (test, user) => new {Test = test, User = user})
                .Where(joined => joined.User.NormalizedUsername == username)
                .Select(joined => joined.Test);
        }
    
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
            .Include(x => x.Language)
            .AsNoTracking()
            .ToListAsync();
        
        return new PaginationHelper<Test> { Items = items, TotalItems = totalItems };
    }
}