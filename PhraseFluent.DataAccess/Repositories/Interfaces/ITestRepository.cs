using PhraseFluent.DataAccess.Entities;

namespace PhraseFluent.DataAccess.Repositories.Interfaces;

public interface ITestRepository : IBaseRepository
{
    public Task<IEnumerable<Test>> GetTestList(int page, int size, string? language, string? username, string? title);
}