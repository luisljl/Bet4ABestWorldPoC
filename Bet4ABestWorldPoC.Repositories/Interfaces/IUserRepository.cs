using Bet4ABestWorldPoC.Repositories.Entities;

namespace Bet4ABestWorldPoC.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
    }
}
