namespace PhraseFluent.Service.DTO.Requests;

public class RefreshTokenRequest
{
    public required string AccessToken { get; set; }
    
    public required string RefreshToken { get; set; }
}