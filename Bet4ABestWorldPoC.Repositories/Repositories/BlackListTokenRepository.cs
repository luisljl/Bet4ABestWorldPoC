using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Repositories.Repositories
{
    public class BlackListTokenRepository : IBlackListTokenRepository
    {
        private readonly AppDbContext _dbContext;

        public BlackListTokenRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task CreateAsync(BlackListToken token)
        {
            await _dbContext.Set<BlackListToken>().AddAsync(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(BlackListToken token)
        {
            _dbContext.Set<BlackListToken>().Remove(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<BlackListToken> FirstOrDefaultAsync(Expression<Func<BlackListToken, bool>> predicate)
        {
            return await _dbContext.Set<BlackListToken>().FirstOrDefaultAsync(predicate);
        }
    }
}
