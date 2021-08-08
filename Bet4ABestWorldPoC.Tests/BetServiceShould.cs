using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Enums;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Services.Request;
using Bet4ABestWorldPoC.Services.Responses;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Bet4ABestWorldPoC.Services.Tests
{
    public class BetServiceShould
    {
        private const int DEFAULT_SLOT_ID = 1;
        private const int DEFAULT_USER_ID = 1;

        private readonly List<Bet> DEFAULT_BETS = new()
        {
            new Bet()
            {
                Id = 1,
                Amount = 0.15,
                BetType = (int)BetType.NORMAL,
                CreatedOn = DateTime.Now,
                SlotId = 1,
                UserId = DEFAULT_USER_ID,
                WinningAmount = 1.16,
                WinningBet = true,
            },
            new Bet()
            {
                Id = 2,
                Amount = 0.15,
                BetType = (int)BetType.NORMAL,
                CreatedOn = DateTime.Now,
                SlotId = 1,
                UserId = DEFAULT_USER_ID,
                WinningAmount = 0,
                WinningBet = false,
            },
            new Bet()
            {
                Id = 3,
                Amount = 0.35,
                BetType = (int)BetType.NORMAL,
                CreatedOn = DateTime.Now,
                SlotId = 1,
                UserId = DEFAULT_USER_ID,
                WinningAmount = 3,
                WinningBet = true,
            },
            new Bet()
            {
                Id = 4,
                Amount = 0.15,
                BetType = (int)BetType.NORMAL,
                CreatedOn = DateTime.Now,
                SlotId = 2,
                UserId = DEFAULT_USER_ID,
                WinningAmount = 0,
                WinningBet = false,
            },
            new Bet()
            {
                Id = 5,
                Amount = 0.15,
                BetType = (int)BetType.NORMAL,
                CreatedOn = DateTime.Now,
                SlotId = 2,
                UserId = 2,
                WinningAmount = 0,
                WinningBet = false,
            },
        };

        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IBalanceService> _mockBalanceService;
        private readonly Mock<IBetRepository> _mockBetRepository;
        private readonly Mock<ISlotService> _mockSlotService;

        private readonly BetService _betService;

        public BetServiceShould()
        {
            _mockTokenService = new Mock<ITokenService>();
            _mockBalanceService = new Mock<IBalanceService>();
            _mockBetRepository = new Mock<IBetRepository>();
            _mockSlotService = new Mock<ISlotService>();

            _betService = new BetService(_mockBetRepository.Object, _mockTokenService.Object, _mockBalanceService.Object, _mockSlotService.Object);
        }

        [Fact]
        public void Throw_invalid_bet_data_exception_if_request_is_null()
        {
            Func<Task> action = async () => await _betService.Bet(null);

            action.Should().Throw<InvalidBetDataException>();
        }

        [Fact]
        public void Throw_invalid_bet_type_exception_if_bet_type_is_not_valid()
        {
            var inventedBetType = 3;

            var request = new BetRequest(DEFAULT_SLOT_ID, 0.10, inventedBetType);

            Func<Task> action = async () => await _betService.Bet(request);

            action.Should().Throw<InvalidBetTypeException>();
        }

        [Fact]
        public void Not_throw_any_exception_when_bet_is_created()
        {
            var request = new BetRequest(DEFAULT_SLOT_ID, 0.10, (int)BetType.NORMAL);

            var expectedBalance = new BalanceResponse()
            {
                CurrentBalance = 14.23
            };

            _mockTokenService.Setup(x => x.GetCurrentUserId()).Returns(DEFAULT_USER_ID);
            _mockBalanceService.Setup(x => x.GetCurrentUserCurrentBalanceAsync()).ReturnsAsync(expectedBalance);

            Func<Task> action = async () => await _betService.Bet(request);

            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public async Task Return_bet_response_when_bet_is_created()
        {
            var request = new BetRequest(DEFAULT_SLOT_ID, 0.10, (int)BetType.NORMAL);

            _mockTokenService.Setup(x => x.GetCurrentUserId()).Returns(DEFAULT_USER_ID);
            _mockBalanceService.Setup(x => x.GetCurrentUserCurrentBalanceAsync()).ReturnsAsync(new BalanceResponse() { CurrentBalance = 10.30 });

            var result = await _betService.Bet(request);

            var expectedResponse = new BetResponse()
            {
                CurrentBalance = 10.30
            };

            result.CurrentBalance.Should().Be(expectedResponse.CurrentBalance);
        }

        [Fact]
        public async Task Return_a_list_of_bets_filtered_by_current_user_and_slot()
        {
            var userBets = DEFAULT_BETS.Where(w => w.UserId == DEFAULT_USER_ID && w.SlotId == DEFAULT_SLOT_ID).ToList();

            var slot1Name = "Slot 1";

            var expectedBet = new UserBetHistoricResponse()
            {
                Bets = userBets.Select(s => new BetHistoricResponse()
                {
                    Id = s.Id,
                    BetAmount = s.Amount,
                    BetType = s.BetType,
                    CreatedOn = s.CreatedOn,
                    SlotId = s.SlotId,
                    WinningAmount = s.WinningAmount,
                    WinningBet = s.WinningBet
                }).ToList(),
                SlotName = slot1Name,
            };

            _mockTokenService.Setup(x => x.GetCurrentUserId()).Returns(DEFAULT_USER_ID);
            _mockBetRepository.Setup(x => x.GetAllWhereAsync(w => w.UserId == DEFAULT_USER_ID && w.SlotId == DEFAULT_SLOT_ID)).ReturnsAsync(userBets);
            _mockSlotService.Setup(x => x.GetSlotNameById(DEFAULT_SLOT_ID)).ReturnsAsync(slot1Name);

            var result = await _betService.GetCurrentUserBetHistoricBySlotAsync(DEFAULT_SLOT_ID);

            result.Should().BeEquivalentTo(expectedBet);
        }

        [Fact]
        public async Task Return_a_list_of_winning_bets_filtered_by_current_user_and_slot()
        {
            var userBets = DEFAULT_BETS.Where(w => w.UserId == DEFAULT_USER_ID && w.SlotId == DEFAULT_SLOT_ID && w.WinningBet).ToList();

            var slot1Name = "Slot 1";

            var expectedBet = new UserBetHistoricResponse()
            {
                Bets = userBets.Select(s => new BetHistoricResponse()
                {
                    Id = s.Id,
                    BetAmount = s.Amount,
                    BetType = s.BetType,
                    CreatedOn = s.CreatedOn,
                    SlotId = s.SlotId,
                    WinningAmount = s.WinningAmount,
                    WinningBet = s.WinningBet
                }).ToList(),
                SlotName = slot1Name,
            };

            _mockTokenService.Setup(x => x.GetCurrentUserId()).Returns(DEFAULT_USER_ID);
            _mockBetRepository.Setup(x => x.GetAllWhereAsync(w => w.UserId == DEFAULT_USER_ID && w.SlotId == DEFAULT_SLOT_ID && w.WinningBet)).ReturnsAsync(userBets);
            _mockSlotService.Setup(x => x.GetSlotNameById(DEFAULT_SLOT_ID)).ReturnsAsync(slot1Name);

            var result = await _betService.GetCurrentUserWinningBetHistoricBySlotAsync(DEFAULT_SLOT_ID);

            result.Should().BeEquivalentTo(expectedBet);
        }

        [Fact]
        public async Task Return_a_list_of_bets_filtered_by_current_user()
        {
            var userBets = DEFAULT_BETS.Where(w => w.UserId == DEFAULT_USER_ID).ToList();

            var slot1 = new Slot()
            {
                Id = 1,
                Name = "Slot 1",
            };

            var slot2 = new Slot()
            {
                Id = 2,
                Name = "Slot 2"
            };

            var expectedBets = new List<UserBetHistoricResponse>()
            {

                new UserBetHistoricResponse()
                {
                    Bets = userBets.Where(w => w.SlotId == slot1.Id).Select(s => new BetHistoricResponse()
                    {
                        Id = s.Id,
                        BetAmount = s.Amount,
                        BetType = s.BetType,
                        CreatedOn = s.CreatedOn,
                        SlotId = s.SlotId,
                        WinningAmount = s.WinningAmount,
                        WinningBet = s.WinningBet
                    }).ToList(),
                    SlotName = slot1.Name,
                },
                new UserBetHistoricResponse()
                {
                    Bets = userBets.Where(w => w.SlotId == slot2.Id).Select(s => new BetHistoricResponse()
                    {
                        Id = s.Id,
                        BetAmount = s.Amount,
                        BetType = s.BetType,
                        CreatedOn = s.CreatedOn,
                        SlotId = s.SlotId,
                        WinningAmount = s.WinningAmount,
                        WinningBet = s.WinningBet
                    }).ToList(),
                    SlotName = slot2.Name,
                },
            };

            _mockTokenService.Setup(x => x.GetCurrentUserId()).Returns(DEFAULT_USER_ID);
            _mockBetRepository.Setup(x => x.GetAllWhereAsync(w => w.UserId == DEFAULT_USER_ID)).ReturnsAsync(userBets);
            _mockSlotService.Setup(x => x.GetSlotNameById(slot1.Id)).ReturnsAsync(slot1.Name);
            _mockSlotService.Setup(x => x.GetSlotNameById(slot2.Id)).ReturnsAsync(slot2.Name);

            var result = await _betService.GetCurrentUserBetHistoricAsync();

            result.Should().BeEquivalentTo(expectedBets);
            
        }

    }
}
