using AutoMapper;
using PhraseFluent.DataAccess.Entities;
using PhraseFluent.DataAccess.Repositories.Interfaces;
using PhraseFluent.Service.DTO.Responses;
using PhraseFluent.Service.Exceptions;

namespace PhraseFluent.Service;

public class UserService (IUserRepository userRepository, IMapper mapper) : IUserService
{
    public async Task UpdateUserImage(string imageUrl, Guid? userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        var user = userRepository.GetByUuid<User>(userId.Value) ?? throw new ForbiddenException("Unable to find user");
        user.ImageUrl = imageUrl;
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();
    }
    
    public Task<UserResponse> GetUserByUuid(Guid? userId, Guid? tokenUserId)
    {
        ArgumentNullException.ThrowIfNull(tokenUserId);
        var user = userRepository.GetByUuid<User>(userId ?? tokenUserId.Value) ?? throw new ArgumentException("Unable to find user");
        var userResponse = mapper.Map<UserResponse>(user);
        return Task.FromResult(userResponse);
    }
}