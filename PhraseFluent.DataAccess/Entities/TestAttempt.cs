using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFluent.DataAccess.Entities;

public class TestAttempt : BaseId
{
    [ForeignKey(nameof(Test))]
    public long TestId { get; set; }
    
    [ForeignKey(nameof(User))]
    public long UserId { get; set; }
    
    public int CorrectAnswers { get; set; }
    
    public int WrongAnswers { get; set; }
    
    public int PartiallyCorrectAnswers { get; set; }
    
    public int OverallResult { get; set; }
    
    public DateTimeOffset StartDate { get; set; }
    
    public DateTimeOffset? EndDate { get; set; }
    
    public ICollection<AnswerAttempt> AnswerAttempts { get; set; }
    
    public Test Test { get; set; }
    
    public User User { get; set; }
}