using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistLearning.DataAccess.Entities;

public class AnswerOption : BaseId
{
    [StringLength(255)] public required string OptionText { get; set; }

    [ForeignKey(nameof(Card))]
    public long CardId { get; set; }
    
    public required bool IsAllowedText { get; set; }
    
    public Card Card { get; set; }
}