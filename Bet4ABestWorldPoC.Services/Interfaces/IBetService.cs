using Bet4ABestWorldPoC.Services.Request;
using Bet4ABestWorldPoC.Services.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface IBetService
    {
        Task<BetResponse> Bet(BetRequest request);
        Task<List<UserBetHistoricResponse>> GetCurrentUserBetHistoricAsync();
        Task<UserBetHistoricResponse> GetCurrentUserBetHistoricBySlotAsync(int slotId);
        Task<UserBetHistoricResponse> GetCurrentUserWinningBetHistoricBySlotAsync(int slotId);
    }
}
