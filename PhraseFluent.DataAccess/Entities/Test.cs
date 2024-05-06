using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhraseFluent.DataAccess.Entities;

public class Test : BaseId
{
    [StringLength(255)]
    public string Title { get; set; }
    
    [StringLength(1000)]
    public string Description { get; set; }
    
    [ForeignKey(nameof(CreatedBy))]
    public long UserId { get; set; }
    
    public int CardsCount { get; set; }
    
    public User CreatedBy { get; set; }
    
    public virtual ICollection<Card> Cards { get; set; }
}