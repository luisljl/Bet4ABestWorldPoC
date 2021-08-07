using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Enums;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Services.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services
{
    public class DepositService : IDepositService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IDepositRepository _depositRepository;

        public DepositService(ITokenService tokenService, IUserService userService, IDepositRepository depositRepository)
        {
            _tokenService = tokenService;
            _userService = userService;
            _depositRepository = depositRepository;
        }

        public async Task DepositAsync(DepositRequest request)
        {
            ValidateDepositRequest(request);
            var userId = _tokenService.GetCurrentUserId();
            var user = await _userService.GetByIdAsync(userId);

            var newDeposit = MapNewDepositFromRequest(request, user);

            await _depositRepository.CreateAsync(newDeposit);
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
                throw new InvalidMerchantIdException();
            }
        }

        private Deposit MapNewDepositFromRequest(DepositRequest request, User user)
        {
            return new Deposit()
            {
                Amount = request.Amount,
                UserId = user.Id,
                MerchantId = request.MerchantId
            };
        }

        public async Task<List<Deposit>> GetDepositsForUserAsync(int userId)
        {
            return await _depositRepository.GetAllWhereAsync(w => w.UserId == userId);
        }
    }
}
