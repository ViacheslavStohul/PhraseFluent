using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PhraseFluent.DataAccess.Enums;

namespace PhraseFluent.DataAccess.Entities;

public class Card : BaseId
{
    [StringLength(255)]
    public required string Question { get; set; }
    
    [ForeignKey(nameof(Test))]
    public long TestId { get; set; }

    public Test Test;
    
    public QuestionType QuestionType { get; set; }
    
    public virtual ICollection<AnswerOption> AnswerOptions { get; set; }
}