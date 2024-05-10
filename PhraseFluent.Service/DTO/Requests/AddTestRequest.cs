namespace PhraseFluent.Service.DTO.Requests;

public class AddTestRequest
{
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? ImageUrl { get; set; }

    public required Guid LanguageUuid { get; set; }
}