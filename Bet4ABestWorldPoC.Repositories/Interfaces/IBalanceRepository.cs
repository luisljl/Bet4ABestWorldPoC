using Bet4ABestWorldPoC.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Repositories.Interfaces
{
    public interface IBalanceRepository
    {
        Task CreateAsync(Balance balance);
        Task<Balance> FirstOrDefaultAsync(Expression<Func<Balance, bool>> predicate);
        Task UpdateAsync(Balance balance);
    }
}
