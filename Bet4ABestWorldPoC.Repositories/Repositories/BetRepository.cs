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
    public class BetRepository : IBetRepository
    {
        private readonly AppDbContext _dbContext;

        public BetRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task CreateAsync(Bet bet)
        {
            await _dbContext.Set<Bet>().AddAsync(bet);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Bet>> GetAllWhereAsync(Expression<Func<Bet, bool>> predicate)
        {
            return await _dbContext.Set<Bet>().AsNoTracking().Where(predicate).ToListAsync();
        }
    }
}
