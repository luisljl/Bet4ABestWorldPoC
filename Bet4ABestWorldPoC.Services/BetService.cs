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
    public class BetService : IBetService
    {
        private readonly IBetRepository _betRepository;
        private readonly ITokenService _tokenService;
        private readonly IBalanceService _balanceService;
        private readonly ISlotService _slotService;

        public BetService(IBetRepository betRepository, ITokenService tokenService, IBalanceService balanceService, ISlotService slotService)
        {
            _betRepository = betRepository;
            _tokenService = tokenService;
            _balanceService = balanceService;
            _slotService = slotService;
        }

        public async Task<BetResponse> Bet(BetRequest request)
        {
            ValidateBetRequest(request);
            var currentUserId = _tokenService.GetCurrentUserId();
            await _balanceService.DecreaseBalanceAsync(request.Amount);
            var slot = await _slotService.GetSlotByIdAsync(request.SlotId);
            var newBet = MapNewBetFromRequest(request, currentUserId, slot);
            await _betRepository.CreateAsync(newBet);
            if (newBet.WinningBet)
            {
                await _balanceService.IncreaseBalanceAsync(newBet.WinningAmount);
            }
            var currentBalance = await _balanceService.GetCurrentUserCurrentBalanceAsync();
            return new BetResponse()
            {
                WinningBet = newBet.WinningBet,
                WinAmount = newBet.WinningAmount,
                CurrentBalance = currentBalance.CurrentBalance,
            };
        }

        public async Task<UserBetHistoricResponse> GetCurrentUserBetHistoricBySlotAsync(int slotId)
        {
            var currentUserId = _tokenService.GetCurrentUserId();
            var bets = await _betRepository.GetAllWhereAsync(w => w.UserId == currentUserId && w.SlotId == slotId);

            return new UserBetHistoricResponse()
            {
                Bets = MapListOfBetToListOfBetHistoricResponse(bets),
                SlotName = await _slotService.GetSlotNameById(slotId)
            };
        }

        public async Task<UserBetHistoricResponse> GetCurrentUserWinningBetHistoricBySlotAsync(int slotId)
        {
            var currentUserId = _tokenService.GetCurrentUserId();
            var bets = await _betRepository.GetAllWhereAsync(w => w.UserId == currentUserId && w.SlotId == slotId && w.WinningBet);

            return new UserBetHistoricResponse()
            {
                Bets = MapListOfBetToListOfBetHistoricResponse(bets),
                SlotName = await _slotService.GetSlotNameById(slotId)
            };
        }

        public async Task<List<UserBetHistoricResponse>> GetCurrentUserBetHistoricAsync()
        {
            var currentUserId = _tokenService.GetCurrentUserId();
            var bets = await _betRepository.GetAllWhereAsync(w => w.UserId == currentUserId);

            var groupedBets = new List<UserBetHistoricResponse>();

            bets.GroupBy(g => g.SlotId).ToList().ForEach(async (group) =>
            {
                groupedBets.Add(new UserBetHistoricResponse()
                {
                    Bets = MapListOfBetToListOfBetHistoricResponse(group.ToList()),
                    SlotName = await _slotService.GetSlotNameById(group.Key),
                });
            });

            return groupedBets;
        }

        private void ValidateBetRequest(BetRequest request)
        {
            if (request == null)
            {
                throw new InvalidBetDataException();
            }

            if (!Enum.IsDefined(typeof(BetType), request.BetTypeId))
            {
                throw new InvalidBetTypeException();
            }
        }

        private Bet MapNewBetFromRequest(BetRequest request, int userId, Slot slot)
        {
            var winning = false;
            if (new Random().Next(1, 100) > slot.RTP)
            {
                winning = true;
            }
            return new Bet()
            {
                SlotId = request.SlotId,
                Amount = request.Amount,
                BetType = request.BetTypeId,
                CreatedOn = DateTime.Now,
                UserId = userId,
                WinningAmount = winning ? SimulateWin(request.Amount) : 0,
                WinningBet = winning,
            };
        }

        private double SimulateWin(double amount)
        {
            return amount * new Random().Next(1, 10);
        }

        private List<BetHistoricResponse> MapListOfBetToListOfBetHistoricResponse(IEnumerable<Bet> bets)
        {
            bets ??= Enumerable.Empty<Bet>();
            return bets.Select(s => new BetHistoricResponse()
            {
                Id = s.Id,
                BetAmount = s.Amount,
                WinningBet = s.WinningBet,
                WinningAmount = s.WinningAmount,
                SlotId = s.SlotId,
                CreatedOn = s.CreatedOn,
                BetType = s.BetType,
            }).ToList();
        }
    }
}
