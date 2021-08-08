using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Repositories.Repositories
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly AppDbContext _dbContext;

        public BalanceRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task CreateAsync(Balance balance)
        {
            await _dbContext.Set<Balance>().AddAsync(balance);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Balance> FirstOrDefaultAsync(Expression<Func<Balance, bool>> predicate)
        {
            return await _dbContext.Set<Balance>().FirstOrDefaultAsync(predicate);
        }

        public async Task UpdateAsync(Balance balance)
        {
            _dbContext.Set<Balance>().Update(balance);
            await _dbContext.SaveChangesAsync();
        }
    }
}
