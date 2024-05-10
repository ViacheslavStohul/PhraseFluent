using PhraseFluent.DataAccess.Entities;

namespace PhraseFluent.DataAccess.Repositories.Interfaces;

public interface ILanguageRepository : IBaseRepository
{
    Task<IEnumerable<Language>> GetAll();
}