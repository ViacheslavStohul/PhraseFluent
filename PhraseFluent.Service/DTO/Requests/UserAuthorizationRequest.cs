namespace PhraseFluent.Service.DTO.Requests;

public class UserAuthorizationRequest
{
    public required string Username { get; set; }
    
    public required string Password { get; set; }
}