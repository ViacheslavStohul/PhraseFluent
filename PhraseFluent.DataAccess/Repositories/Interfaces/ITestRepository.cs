using PhraseFluent.DataAccess.Entities;
using PhraseFluent.DataAccess.Helpers;

namespace PhraseFluent.DataAccess.Repositories.Interfaces;

public interface ITestRepository : IBaseRepository
{
    public Task<TestSearcHelper> GetTestList(int page, int size, string? language, string? username, string? title);
}