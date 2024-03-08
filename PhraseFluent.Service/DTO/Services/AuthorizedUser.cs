using PhraseFluent.Service.DTO.Requests;

namespace PhraseFluent.Service.DTO.Services;

public class AuthorizedUser
{
    public required string Username { get; set; }
    
    public required string Password { get; set; }

    public static implicit operator AuthorizedUser(UserAuthorizationRequest request)
    {
        return new AuthorizedUser { Username = request.Username, Password = request.Password };
    }
}