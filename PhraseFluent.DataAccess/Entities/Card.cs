using System.ComponentModel.DataAnnotations;
using PhraseFluent.DataAccess.Enums;

namespace PhraseFluent.DataAccess.Entities;

public class Card : BaseId
{
    [StringLength(255)]
    public required string Question { get; set; }
    
    public QuestionType QuestionType { get; set; }
}