using System.ComponentModel.DataAnnotations;

namespace PhraseFluent.DataAccess.Entities;

public class AnswerOption : BaseId
{
    [StringLength(255)] public required string OptionText { get; set; }

    [StringLength(255)] public required bool IsCorrect;
}