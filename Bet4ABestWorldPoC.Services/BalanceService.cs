using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Services.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly ITokenService _tokenService;

        public BalanceService(IBalanceRepository balanceRepository, ITokenService tokenService)
        {
            _balanceRepository = balanceRepository;
            _tokenService = tokenService;
        }

        public async Task CreateAsync(int userId)
        {
            await _balanceRepository.CreateAsync(new Repositories.Entities.Balance()
            {
                Amount = 0,
                LastUpdate = DateTime.Now,
                UserId = userId
            });
        }

        public async Task IncreaseBalanceAsync(double amount)
        {
            ValidateAmount(amount);
            
            var userId = _tokenService.GetCurrentUserId();
            var balance = await _balanceRepository.FirstOrDefaultAsync(w => w.UserId == userId);
            
            if (balance == null)
            {
                throw new BalanceNotFoundException();
            }
            balance.Amount += amount;
            balance.LastUpdate = DateTime.Now;

            await _balanceRepository.UpdateAsync(balance);
        }

        public async Task<BalanceResponse> GetCurrentUserCurrentBalanceAsync()
        {
            var userId = _tokenService.GetCurrentUserId();

            var balance = await _balanceRepository.FirstOrDefaultAsync(w => w.UserId == userId);

            return new BalanceResponse()
            {
                CurrentBalance = balance.Amount,
            };
        }

        public async Task DecreaseBalanceAsync(double amount)
        {
            ValidateAmount(amount);

            var userId = _tokenService.GetCurrentUserId();
            var balance = await _balanceRepository.FirstOrDefaultAsync(w => w.UserId == userId);

            if (balance == null)
            {
                throw new BalanceNotFoundException();
            }
            if (balance.Amount < amount)
            {
                throw new InsufficientBalanceException();
            }
            balance.Amount -= amount;
            balance.LastUpdate = DateTime.Now;

            await _balanceRepository.UpdateAsync(balance);
        }

        private void ValidateAmount(double amount)
        {
            if (amount < 0)
            {
                throw new InvalidBalanceDataException();
            }
        }
    }
}
