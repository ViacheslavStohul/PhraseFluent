namespace PhraseFluent.Service.DTO.Requests;

public class UserCreationRequest
{
    public required string Username { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public required string Password { get; set; }
    
    public required string RepeatedPassword { get; set; }
}