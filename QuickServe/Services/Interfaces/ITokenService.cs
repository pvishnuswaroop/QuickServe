using QuickServe.Models;

namespace QuickServe.Services.Interfaces
{
    public interface ITokenService
    {
        Task SaveRefreshToken(string username, string token);
        Task<string> RetrieveUsernameByRefreshToken(string refreshToken);
        Task<bool> RevokeRefreshToken(string refreshToken);

        // Only one definition of each method
        string GenerateRefreshToken();
        string GenerateJwtToken(User user);
    }
}
