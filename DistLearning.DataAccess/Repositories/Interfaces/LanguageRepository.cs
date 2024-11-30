using DistLearning.DataAccess.Entities;

namespace DistLearning.DataAccess.Repositories.Interfaces;

public interface ILanguageRepository : IBaseRepository
{
    Task<IEnumerable<Language>> GetAll();
}