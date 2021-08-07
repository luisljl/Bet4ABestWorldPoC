using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Services.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface IDepositService
    {
        Task DepositAsync(DepositRequest request);
        Task<List<Deposit>> GetDepositsForUserAsync(int userId);
    }
}
