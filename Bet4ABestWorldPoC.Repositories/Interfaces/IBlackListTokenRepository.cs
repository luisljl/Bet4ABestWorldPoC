using Bet4ABestWorldPoC.Repositories.Entities;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Repositories.Interfaces
{
    public interface IBlackListTokenRepository
    {
        Task Add(BlackListToken token);
        Task Delete(BlackListToken token);
        Task<BlackListToken> Get(string token);
    }
}
