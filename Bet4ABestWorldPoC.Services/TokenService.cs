using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(User user)
        {
            throw new NotImplementedException();
        }

        public Task InvalidateToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
