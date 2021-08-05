using Bet4ABestWorldPoC.Repositories.Entities;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByUsername(string username);
        Task Save(User user);
    }
}
