using Microsoft.EntityFrameworkCore;
using PhraseFluent.DataAccess.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace PhraseFluent.DataAccess;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; }
    
    public virtual DbSet<UserSession> UserSessions { get; set; }

    public async Task<int> Initialize()
    {
        var changesRowsCount = 0;
        using (var transaction = await Database.BeginTransactionAsync())
        {
            try
            {
                var scriptsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Migrations", "Scripts");
                var scriptFiles = Directory.GetFiles(scriptsDirectory, "*.sql");

                foreach (var scriptFile in scriptFiles)
                {
                    var scriptContent = await File.ReadAllTextAsync(scriptFile);
                    await Database.ExecuteSqlRawAsync(scriptContent);
                    changesRowsCount += await SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        return changesRowsCount;
    }
}