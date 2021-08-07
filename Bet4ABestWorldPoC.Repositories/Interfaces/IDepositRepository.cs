using Bet4ABestWorldPoC.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Repositories.Interfaces
{
    public interface IDepositRepository
    {
        Task CreateAsync(Deposit deposit);
        Task<List<Deposit>> GetAllWhereAsync(Expression<Func<Deposit, bool>> predicate);
    }
}
