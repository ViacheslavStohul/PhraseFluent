using System.ComponentModel.DataAnnotations;

namespace PhraseFluent.DataAccess.Entities;

public class CompleteInitializer
{
    [Key]
    public Guid CompleteInitializerId { get; set; }
    
    [StringLength(100)]
    public string? CompleteInitializerName { get; set; }
}