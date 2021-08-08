using Bet4ABestWorldPoC.Services.Request;
using Bet4ABestWorldPoC.Services.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface IDepositService
    {
        Task DepositAsync(DepositRequest request);
        Task<List<DepositHistoricResponse>> GetDepositsForCurrentUserAsync();
    }
}
