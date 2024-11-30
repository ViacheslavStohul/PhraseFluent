using Microsoft.EntityFrameworkCore;
using DistLearning.DataAccess.Entities;
using DistLearning.DataAccess.Repositories.Interfaces;

namespace DistLearning.DataAccess.Repositories;

public class LanguageRepository(DataContext dataContext) : BaseRepository(dataContext), ILanguageRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<IEnumerable<Language>> GetAll()
    {
        return await _dataContext.Languages.ToListAsync();
    }
}