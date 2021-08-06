using Bet4ABestWorldPoC.Repositories.Entities;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetByIdAsync(int id);
    }
}
