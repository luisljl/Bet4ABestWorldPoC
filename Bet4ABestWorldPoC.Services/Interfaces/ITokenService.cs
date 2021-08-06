using Bet4ABestWorldPoC.Repositories.Entities;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        Task InvalidateToken(string token);
        Task<BlackListToken> GetInvalidToken(string token);
    }
}
