using Bet4ABestWorldPoC.Services.Responses;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface IBalanceService
    {
        public Task CreateAsync(int userId);
        public Task<BalanceResponse> GetCurrentUserCurrentBalanceAsync();
        public Task IncreaseBalanceAsync(double amount);
        public Task DecreaseBalanceAsync(double amount);
    }
}
