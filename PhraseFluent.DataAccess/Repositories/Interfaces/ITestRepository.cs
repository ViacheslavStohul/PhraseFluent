using PhraseFluent.DataAccess.Entities;
using PhraseFluent.DataAccess.Helpers;

namespace PhraseFluent.DataAccess.Repositories.Interfaces;

public interface ITestRepository : IBaseRepository
{
    public Task<PaginationHelper<Test>> GetTestList(int page, int size, string? language, string? username, string? title);

    Task<Test?> TestWithCards(Guid testUuid);
}