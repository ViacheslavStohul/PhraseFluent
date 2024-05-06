using System.ComponentModel.DataAnnotations;

namespace PhraseFluent.DataAccess.Entities;

public class CardOption : BaseId
{
    [StringLength(255)] public required string OptionText { get; set; }

    [StringLength(255)] public required bool IsCorrect;
}