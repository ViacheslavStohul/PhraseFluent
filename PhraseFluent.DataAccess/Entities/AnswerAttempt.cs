using System.ComponentModel.DataAnnotations.Schema;
using PhraseFluent.DataAccess.Enums;

namespace PhraseFluent.DataAccess.Entities;

public class AnswerAttempt : BaseId
{
    [ForeignKey(nameof(PickedAnswer))]
    public long TestAttemptId { get; set; }
    
    [ForeignKey(nameof(AnswerOption))]
    public long? AnswerOptionId { get; set; }
    
    [ForeignKey(nameof(Card))]
    public long CardId { get; set; }
    
    public AnswerResult AnswerResult { get; set; }
    
    public required TestAttempt PickedAnswer { get; set; }
    
    public AnswerOption? AnswerOption { get; set; }
    
    public required Card Card { get; set; }
}