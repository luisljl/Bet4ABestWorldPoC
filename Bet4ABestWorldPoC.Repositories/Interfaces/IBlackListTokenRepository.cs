using Bet4ABestWorldPoC.Repositories.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Repositories.Interfaces
{
    public interface IBlackListTokenRepository
    {
        Task CreateAsync(BlackListToken token);
        Task DeleteAsync(BlackListToken token);
        Task<BlackListToken> FirstOrDefaultAsync(Expression<Func<BlackListToken, bool>> predicate);
    }
}
