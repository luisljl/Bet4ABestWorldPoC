using Bet4ABestWorldPoC.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface IUserService
    {
        Task Create(User user);
        Task<User> GetUserByUsername(string username);
        Task<User> GetById(int id);
    }
}
