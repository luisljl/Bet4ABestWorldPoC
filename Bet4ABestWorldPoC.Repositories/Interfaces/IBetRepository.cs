using Bet4ABestWorldPoC.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Repositories.Interfaces
{
    public interface IBetRepository
    {
        Task CreateAsync(Bet bet);
        Task<List<Bet>> GetAllWhereAsync(Expression<Func<Bet, bool>> predicate);
    }
}
