using DistLearning.DataAccess.Entities;

namespace DistLearning.DataAccess;

using Microsoft.EntityFrameworkCore;

public partial class DataContext 
{
    public async Task Initialize()
    {
        await using var transaction = await Database.BeginTransactionAsync();
        try
        {
            var completedMigrations = CompleteInitializers.Select(x => x.CompleteInitializerId);
            
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}