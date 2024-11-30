using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistLearning.DataAccess.Entities;

public class TestAttempt : BaseId
{
    [ForeignKey(nameof(Test))]
    public long TestId { get; set; }
    
    [ForeignKey(nameof(User))]
    public long UserId { get; set; }
    
    public bool Completed { get; set; }
    
    public DateTimeOffset StartDate { get; set; }
    
    public DateTimeOffset? EndDate { get; set; }
    
    public ICollection<AnswerAttempt> AnswerAttempts { get; set; }
    
    public Test Test { get; set; }
    
    public User User { get; set; }
    
    [StringLength(200)]
    public required string QuestionOrder { get; set; }
}