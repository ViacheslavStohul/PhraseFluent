using PhraseFluent.DataAccess.Entities;
using PhraseFluent.Service.DTO.Requests;
using PhraseFluent.Service.Extensions;

namespace PhraseFluent.Service.DTO.Services;

public class CreatedUser
{
    public required string Username { get; set; }
    
    public required string Password { get; set; }
    
    public required string RepeatedPassword { get; set; }
    
    public static implicit operator User(CreatedUser? createdUser)
    {
        ArgumentNullException.ThrowIfNull(createdUser);

        return new User 
        {
            Uuid = Guid.NewGuid(),
            Username = createdUser.Username,
            ClientSecret = createdUser.Password.Hash(),
            RegistrationDate = DateTime.UtcNow,
        };
    }

    public static implicit operator CreatedUser(UserCreationRequest request)
    {
        return new CreatedUser 
        {
            Username = request.Username,
            Password = request.Password,
            RepeatedPassword = request.RepeatedPassword
        };
    }
}