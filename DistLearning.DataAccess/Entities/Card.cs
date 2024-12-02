using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DistLearning.DataAccess.Enums;

namespace DistLearning.DataAccess.Entities;

public class Card : BaseId
{
    [StringLength(3000)]
    public required string Question { get; set; }
    
    [ForeignKey(nameof(Test))]
    public long TestId { get; set; }

    public Test Test { get; set; }
    
    public QuestionType QuestionType { get; set; }
    
    public virtual ICollection<AnswerOption> AnswerOptions { get; set; }
}