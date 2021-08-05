using Bet4ABestWorldPoC.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetById(int id);
        Task<User> FirstOrDefault(Expression<Func<User, bool>> predicate);
        Task Add(User user);
    }
}
