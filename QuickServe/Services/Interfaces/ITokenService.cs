using QuickServe.Models;
using System.Threading.Tasks;

namespace QuickServe.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(User user);         // Method to generate JWT token.
        Task<string> GenerateAccessToken(User user);     // Method to generate access token (you can use GenerateJwtToken here).
        string GenerateRefreshToken();                    // Method to generate a refresh token.
        Task SaveRefreshToken(string username, string token);  // Method to save the refresh token in the database.
        Task<string> RetrieveUsernameByRefreshToken(string refreshToken);  // Method to retrieve the username by refresh token.
        Task<bool> RevokeRefreshToken(string refreshToken);  // Method to revoke (delete) the refresh token.
    }
}
