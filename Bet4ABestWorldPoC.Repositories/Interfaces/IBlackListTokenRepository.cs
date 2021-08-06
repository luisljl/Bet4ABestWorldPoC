using Bet4ABestWorldPoC.Repositories.Entities;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Repositories.Interfaces
{
    public interface IBlackListTokenRepository
    {
        Task AddAsync(BlackListToken token);
        Task DeleteAsync(BlackListToken token);
        Task<BlackListToken> GetAsync(string token);
    }
}
