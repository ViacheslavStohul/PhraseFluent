using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistLearning.DataAccess.Entities;

public class Test : BaseId
{
    [StringLength(250)]
    public required string Title { get; set; }
    
    [StringLength(250)]
    public required string NormalizedTitle { get; set; }
    
    [StringLength(1500)]
    public string? Description { get; set; }
    
    [StringLength(255)]
    public string? ImageUrl { get; set; }
    
    [ForeignKey(nameof(CreatedBy))]
    public long UserId { get; set; }
    
    public int CardsCount { get; set; }
    
    public User CreatedBy { get; set; }
    
    public virtual ICollection<Card>? Cards { get; set; }
} 