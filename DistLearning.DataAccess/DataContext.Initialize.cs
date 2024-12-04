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
            var completedMigrations = await CompleteInitializers.Select(x => x.CompleteInitializerId).ToListAsync();

            await RenderTextAnswers(completedMigrations);
            
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task RenderTextAnswers(List<Guid> completeMigrations)
    {
        var migrationId = Guid.Parse("A0A9CA66-181D-471C-8F37-A818AD924904");
        
        if (completeMigrations.Contains(migrationId)) return;

        var unprocessedTextAnswers = AnswerAttempts.Where(x => x.TextAnswer != null && (x.TextAnswer.EndsWith(' ') || x.TextAnswer.StartsWith(' '))).ToList();

        foreach (var answer in unprocessedTextAnswers)
        {
            answer.TextAnswer = answer.TextAnswer!.TrimStart();
            answer.TextAnswer = answer.TextAnswer!.TrimEnd();
        }
        
        completeMigrations.Add(migrationId);

        await SaveChangesAsync();
    }
}