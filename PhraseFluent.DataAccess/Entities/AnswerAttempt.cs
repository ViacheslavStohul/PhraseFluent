using System.ComponentModel.DataAnnotations.Schema;
using PhraseFluent.DataAccess.Enums;

namespace PhraseFluent.DataAccess.Entities;

public class AnswerAttempt : BaseId
{
    [ForeignKey(nameof(TestAttempt))]
    public long TestAttemptId { get; set; }
    
    [ForeignKey(nameof(Card))]
    public long CardId { get; set; }
    
    public AnswerResult AnswerResult { get; set; }
    
    public TestAttempt TestAttempt { get; set; }
    
    public Card Card { get; set; }
}