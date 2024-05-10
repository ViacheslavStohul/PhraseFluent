using System.ComponentModel.DataAnnotations;

namespace PhraseFluent.DataAccess.Entities;

public class Language : BaseId
{
    [StringLength(100)]
    public required string Title { get; set; }
    
    [StringLength(100)]
    public required string NormalizedTitle { get; set; }
    
    [StringLength(100)]
    public required string NativeName { get; set; }
    
    [StringLength(100)]
    public required string NormalizedNativeName { get; set; }
    
    [StringLength(10)]
    public required string LanguageCode { get; set; }
}