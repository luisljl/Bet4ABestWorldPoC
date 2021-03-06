using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Enums;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Services.Request;
using Bet4ABestWorldPoC.Services.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services
{
    public class DepositService : IDepositService
    {
        private readonly ITokenService _tokenService;
        private readonly IDepositRepository _depositRepository;
        private readonly IBalanceService _balanceService;

        public DepositService(ITokenService tokenService, IDepositRepository depositRepository, IBalanceService balanceService)
        {
            _tokenService = tokenService;
            _depositRepository = depositRepository;
            _balanceService = balanceService;
        }

        public async Task DepositAsync(DepositRequest request)
        {
            ValidateDepositRequest(request);
            var userId = _tokenService.GetCurrentUserId();
            var newDeposit = MapNewDepositFromRequest(request, userId);

            await _depositRepository.CreateAsync(newDeposit);
            await _balanceService.IncreaseBalanceAsync(newDeposit.Amount);
        }

        public async Task<List<DepositHistoricResponse>> GetDepositsForCurrentUserAsync()
        {
            var currentUser = _tokenService.GetCurrentUserId();
            var deposits = await _depositRepository.GetAllWhereAsync(w => w.UserId == currentUser);

            if (deposits == null)
            {
                return new List<DepositHistoricResponse>();
            }

            return deposits.Select(s => new DepositHistoricResponse()
            {
                Id = s.Id,
                Amount = s.Amount,
                MerchantId = s.MerchantId
            }).ToList();
        }

        private void ValidateDepositRequest(DepositRequest request)
        {
            if (request == null)
            {
                throw new InvalidDepositDataException();
            }

            if (request.Amount < 10)
            {
                throw new MinimumAmountException();
            }

            if (!Enum.IsDefined(typeof(Merchant), request.MerchantId))
            {
                throw new InvalidMerchantException();
            }
        }

        private Deposit MapNewDepositFromRequest(DepositRequest request, int userId)
        {
            return new Deposit()
            {
                Amount = request.Amount,
                UserId = userId,
                MerchantId = request.MerchantId
            };
        }
    }
}
