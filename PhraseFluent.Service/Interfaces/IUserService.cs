using PhraseFluent.Service.DTO.Responses;

namespace PhraseFluent.Service.Interfaces;

public interface IUserService
{
    Task UpdateUserImage(string imageUrl, Guid? userId);

    Task<UserResponse> GetUserByUuid(Guid? userId, Guid? tokenUserId);
}