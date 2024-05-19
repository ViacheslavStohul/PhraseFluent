namespace PhraseFluent.Service.DTO.Responses;

public class TestResponse
{
    public Guid Uuid { get; set; }
    
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public int CardsAmount { get; set; }
    
    public LanguageResponse? Language { get; set; }
    
    public UserResponse? CreatedBy { get; set; }
}