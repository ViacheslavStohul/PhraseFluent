namespace PhraseFluent.Service.DTO.Responses;

public class UserResponse
{
    public Guid Uuid { get; set; }
    
    public required string Username { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public required DateTime RegistrationDate { get; set; }
}