using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PhraseFluent.DataAccess.Entities;

[Index(nameof(Username), IsUnique = true)]
public class User : BaseId
{
    [StringLength(255)] public required string Username { get; set; }
    
    [StringLength(255)] public required string ClientSecret { get; set; }
    
    public required DateTime RegistrationDate { get; set; }
}