using Bet4ABestWorldPoC.Repositories.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate);
        Task CreateAsync(User user);
    }
}
