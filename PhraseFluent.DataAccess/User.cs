using System.ComponentModel.DataAnnotations;

namespace PhraseFluent.DataAccess;

public class User
{
    public Guid Id { get; set; }

    [StringLength(255)] public required string Username { get; set; }
    
    [StringLength(255)] public required string ClientSecret { get; set; }
}