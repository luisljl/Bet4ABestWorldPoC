using Bet4ABestWorldPoC.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        Task InvalidateToken(string token);
    }
}
