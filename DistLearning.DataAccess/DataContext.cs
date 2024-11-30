using Microsoft.EntityFrameworkCore;
using DistLearning.DataAccess.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace DistLearning.DataAccess;

public partial class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserSession> UserSessions { get; set; }
    
    public virtual DbSet<Test> Tests { get; set; }
    
    public virtual DbSet<Card> Cards { get; set; }
    
    public virtual DbSet<AnswerOption> AnswerOptions { get; set; }
    
    public virtual DbSet<AnswerAttempt> AnswerAttempts { get; set; }
    
    public virtual DbSet<TestAttempt> TestAttempts { get; set; }
    
    public virtual DbSet<CompleteInitializer> CompleteInitializers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnswerAttempt>()
            .HasOne(a => a.TestAttempt)
            .WithMany(t => t.AnswerAttempts)
            .HasForeignKey(a => a.TestAttemptId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AnswerAttempt>()
            .HasOne(a => a.AnswerOption)
            .WithMany()
            .HasForeignKey(a => a.AnswerOptionId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AnswerAttempt>()
            .HasOne(a => a.Card)
            .WithMany()
            .HasForeignKey(a => a.CardId)
            .OnDelete(DeleteBehavior.NoAction);
    }

}