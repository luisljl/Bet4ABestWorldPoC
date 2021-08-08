using Bet4ABestWorldPoC.Services.Request;
using Bet4ABestWorldPoC.Services.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface IBetService
    {
        Task<BetResponse> Bet(BetRequest request);
        Task<List<UserBetHistoryResponse>> GetCurrentUserBetHistoryAsync();
        Task<UserBetHistoryResponse> GetCurrentUserBetHistoryBySlotAsync(int slotId);
        Task<UserBetHistoryResponse> GetCurrentUserWinningBetHistoryBySlotAsync(int slotId);
    }
}
