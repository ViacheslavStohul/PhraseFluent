using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cms.Ecc;
using PhraseFluent.DataAccess.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace PhraseFluent.DataAccess;

public partial class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserSession> UserSessions { get; set; }
    
    public virtual DbSet<Test> Tests { get; set; }
    
    public virtual DbSet<Card> Cards { get; set; }
    
    public virtual DbSet<AnswerOption> AnswerOptions { get; set; }
    
    public virtual DbSet<AnswerAttempt> AnswerAttempts { get; set; }
    
    public virtual DbSet<TestAttempt> TestAttempts { get; set; }
    
    public virtual DbSet<Language> Languages { get; set; }
    
    public virtual DbSet<CompleteInitializer> CompleteInitializers { get; set; }
}