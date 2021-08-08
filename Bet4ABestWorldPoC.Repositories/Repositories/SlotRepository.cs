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
    public class SlotRepository : ISlotRepository
    {
        private readonly AppDbContext _dbContext;

        public SlotRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Slot>> GetAllAsync()
        {
            return await _dbContext.Set<Slot>().AsNoTracking().ToListAsync();
        }

        public async Task<List<Slot>> GetAllWhereAsync(Expression<Func<Slot, bool>> predicate)
        {
            return await _dbContext.Set<Slot>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<Slot> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Slot>().FindAsync(id);
        }
    }
}
