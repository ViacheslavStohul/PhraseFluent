using System.ComponentModel.DataAnnotations;

namespace PhraseFluent.DataAccess.Entities;

public class Card : BaseId
{
    [StringLength(255)]
    public string? Question { get; set; }
}