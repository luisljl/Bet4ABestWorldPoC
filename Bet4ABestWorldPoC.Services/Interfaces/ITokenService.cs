using Bet4ABestWorldPoC.Repositories.Entities;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        Task InvalidateTokenAsync(int userId);
        Task<BlackListToken> GetInvalidTokenAsyncByUserIdAsync(int userId);
        Task DeleteInvalidTokenAsync(int userId);

        string GetCurrentUserToken();
        int GetCurrentUserId();
        string GetCurrentUserEmail();
        string GetCurrentUserUsername();
    }
}
