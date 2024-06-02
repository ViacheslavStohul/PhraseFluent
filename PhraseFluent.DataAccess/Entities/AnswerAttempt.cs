using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhraseFluent.DataAccess.Enums;

namespace PhraseFluent.DataAccess.Entities;

public class AnswerAttempt : BaseId
{
    [ForeignKey(nameof(TestAttempt))]
    public long TestAttemptId { get; set; }
    
    [ForeignKey(nameof(AnswerOption))]
    public long? AnswerOptionId { get; set; }
    
    [ForeignKey(nameof(Card))]
    public long CardId { get; set; }
    
    public AnswerResult AnswerResult { get; set; }
    
    [StringLength(255)] public string? TextAnswer { get; set; }
    
    public required TestAttempt TestAttempt { get; set; }
    
    public AnswerOption? AnswerOption { get; set; }
    
    public required Card Card { get; set; }
}