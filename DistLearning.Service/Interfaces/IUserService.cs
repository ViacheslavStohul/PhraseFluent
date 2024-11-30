using DistLearning.Service.DTO.Responses;

namespace DistLearning.Service.Interfaces;

public interface IUserService
{
    Task UpdateUserImage(string imageUrl, Guid? userId);

    Task<UserResponse> GetUserByUuid(Guid? userId, Guid? tokenUserId);
}