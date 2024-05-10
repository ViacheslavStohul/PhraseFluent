namespace PhraseFluent.Service.DTO.Responses;

public class LanguageResponse
{
    public Guid Uuid { get; set; }
    
    public required string Title { get; set; }
    
    public required string LanguageCode { get; set; }
    
    public required string NativeName { get; set; }
}