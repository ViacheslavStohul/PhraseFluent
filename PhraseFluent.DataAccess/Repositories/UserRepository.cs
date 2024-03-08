using Microsoft.EntityFrameworkCore;
using PhraseFluent.DataAccess.Entities;
using PhraseFluent.DataAccess.Repositories.Interfaces;

namespace PhraseFluent.DataAccess.Repositories;

public class UserRepository(DataContext dataContext) : BaseRepository(dataContext), IUserRepository
{
    private readonly DataContext _dataContext = dataContext;

    /// <summary>
    /// Checks if a username is occupied.
    /// </summary>
    /// <param name="username">The username to check.</param>
    /// <returns><c>true</c> if the username is occupied, <c>false</c> otherwise.</returns>
    public bool IsUserNameOccupied(string username)
    {
        return _dataContext.Users.Any(x => x.Username.ToLower() == username.ToLower());
    }

    /// <summary>
    /// Retrieves a user from the database given a username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>
    /// The user object if found in the database, otherwise null.
    /// </returns>
    public async Task<User?> GetUserByUsername(string username)
    {
        var user = await _dataContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username);

        return user;
    }

    /// <summary>
    /// Retrieves a user session by the provided refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token used to identify the user session.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation. The result is a nullable <see cref="UserSession"/> object if found, otherwise <c>null</c>.</returns>
    public async Task<UserSession?> GetSessionByRefreshToken(string refreshToken)
    {
        var session = await _dataContext.UserSessions.Include(x => x.User)
            .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

        return session;
    }
}