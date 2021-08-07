using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Services.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface IDepositService
    {
        Task DepositAsync(DepositRequest request);
        Task<List<Deposit>> GetDepositsForUserAsync(int userId);
    }
}
