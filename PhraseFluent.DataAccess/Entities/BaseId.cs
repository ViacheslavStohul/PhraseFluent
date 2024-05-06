using System.ComponentModel.DataAnnotations;

namespace PhraseFluent.DataAccess.Entities;

public abstract class BaseId
{
    [Key]
    public long Id { get; set; }
    
    public required Guid Uuid { get; set; }
}