﻿using Bet4ABestWorldPoC.Repositories.Entities;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        Task InvalidateTokenAsync(string token);
        Task<BlackListToken> GetInvalidTokenAsync(string token);
    }
}