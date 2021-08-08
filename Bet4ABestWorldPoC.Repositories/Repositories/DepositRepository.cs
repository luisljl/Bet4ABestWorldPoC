using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Repositories.Repositories
{
    public class DepositRepository : IDepositRepository
    {
        private readonly AppDbContext _dbContext;

        public DepositRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task CreateAsync(Deposit deposit)
        {
            await _dbContext.Set<Deposit>().AddAsync(deposit);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Deposit>> GetAllWhereAsync(Expression<Func<Deposit, bool>> predicate)
        {
            return await _dbContext.Set<Deposit>().AsNoTracking().Where(predicate).ToListAsync();
        }
    }
}
