using Bet4ABestWorldPoC.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Repositories.Interfaces
{
    public interface ISlotRepository
    {
        Task<List<Slot>> GetAll();
        Task<List<Slot>> GetAllWhere(Expression<Func<Slot, bool>> predicate);
        Task<Slot> GetById(int id);
    }
}
