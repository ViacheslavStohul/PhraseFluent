using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFluent.DataAccess.Entities;

public class AnswerOption : BaseId
{
    [StringLength(255)] public required string OptionText { get; set; }

    [StringLength(255)] public required bool IsCorrect;
    
    [ForeignKey(nameof(Card))]
    public long CardId { get; set; }
    
    public Card Card { get; set; }
}