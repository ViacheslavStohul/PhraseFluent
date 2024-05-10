using Microsoft.EntityFrameworkCore;
using PhraseFluent.DataAccess.Entities;
using PhraseFluent.DataAccess.Repositories.Interfaces;

namespace PhraseFluent.DataAccess.Repositories;

public class LanguageRepository(DataContext dataContext) : BaseRepository(dataContext), ILanguageRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<IEnumerable<Language>> GetAll()
    {
        return await _dataContext.Languages.ToListAsync();
    }
}