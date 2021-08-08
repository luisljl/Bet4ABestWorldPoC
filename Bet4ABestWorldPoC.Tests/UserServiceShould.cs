using System;
using Xunit;
using FluentAssertions;
using Bet4ABestWorldPoC.Services.Exceptions;
using Moq;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using System.Threading.Tasks;
using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Services.Responses;
using System.Collections.Generic;
using System.Linq;
using Bet4ABestWorldPoC.Services.Enums;

namespace Bet4ABestWorldPoC.Services.Tests
{
    public class UserServiceShould
    {
        private const int DEFAULT_USER_ID = 1;
        private const string DEFAULT_USERNAME = "test";
        private const string DEFAULT_EMAIL = "test@test.com";
        private const string DEFAULT_PASSWORD = "passtest";

        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IBalanceService> _mockBalanceService;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IBetService> _mockBetService;
        private readonly Mock<IDepositService> _mockDepositService;

        private readonly UserService _userService;

        public UserServiceShould()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockBalanceService = new Mock<IBalanceService>();
            _mockTokenService = new Mock<ITokenService>();
            _mockBetService = new Mock<IBetService>();
            _mockDepositService = new Mock<IDepositService>();

            _userService = new UserService(_mockUserRepository.Object, _mockBalanceService.Object, _mockTokenService.Object, _mockBetService.Object, _mockDepositService.Object);
        }

        [Fact]
        public void Throw_user_not_found_exception_when_id_does_not_exit_searching_by_id()
        {
            var inventedId = 24345;

            _mockUserRepository.Setup(x => x.GetByIdAsync(inventedId)).ReturnsAsync((null as User));

            Func<Task> action = async () => await _userService.GetByIdAsync(inventedId);

            action.Should().Throw<UserNotFoundException>();
        }

        [Fact]
        public void Throw_user_not_found_exception_when_username_does_not_exit_searching_by_username()
        {
            var inventedUsername = "user";
            User expectedUser = null;

            _mockUserRepository.Setup(x => x.FirstOrDefaultAsync(w => w.Username == inventedUsername)).ReturnsAsync(expectedUser);

            Func<Task> action = async () => await _userService.GetUserByUsernameAsync(inventedUsername);

            action.Should().Throw<UserNotFoundException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throw_invalid_username_exception_when_username_is_null_or_empty_searching_by_username(string username)
        {
            var expectedUser = new User()
            {
                Username = username
            };

            _mockUserRepository.Setup(x => x.FirstOrDefaultAsync(w => w.Username == username)).ReturnsAsync(expectedUser);

            Func<Task> action = async () => await _userService.GetUserByUsernameAsync(username);

            action.Should().Throw<InvalidUsernameException>();
        }

        [Theory]
        [InlineData("test3")]
        [InlineData("test5")]
        public async void Return_user_when_username_exists_searching_by_username(string username)
        {
            var expectedUser = new User()
            {
                Username = username
            };

            _mockUserRepository.Setup(x => x.FirstOrDefaultAsync(w => w.Username == username)).ReturnsAsync(expectedUser);

            var result = await _userService.GetUserByUsernameAsync(username);

            result.Username.Should().Be(username);
        }

        [Fact]
        public void Throw_invalid_user_data_exception_when_user_is_null()
        {
            User user = null;

            Func<Task> action = async () => await _userService.CreateAsync(user);

            action.Should().Throw<InvalidUserDataException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throw_invalid_username_exception_when_username_is_empty_or_null(string username)
        {
            var user = new User()
            {
                Username = username,
                Password = DEFAULT_PASSWORD,
                Email = DEFAULT_EMAIL,
            };

            Func<Task> action = async () => await _userService.CreateAsync(user);

            action.Should().Throw<InvalidUsernameException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throw_invalid_password_exception_when_password_is_empty_or_null(string password)
        {
            var user = new User()
            {
                Username = DEFAULT_USERNAME,
                Password = password,
                Email = DEFAULT_EMAIL,
            };

            Func<Task> action = async () => await _userService.CreateAsync(user);

            action.Should().Throw<InvalidPasswordException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throw_invalid_email_exception_when_email_is_empty_or_null(string email)
        {
            var user = new User()
            {
                Username = DEFAULT_USERNAME,
                Password = DEFAULT_PASSWORD,
                Email = email,
            };

            Func<Task> action = async () => await _userService.CreateAsync(user);

            action.Should().Throw<InvalidEmailException>();
        }

        [Fact]
        public void Throw_user_alredy_exists_exception_when_username_exists_in_db()
        {
            var user = new User()
            {
                Username = DEFAULT_USERNAME,
                Email = DEFAULT_EMAIL,
                Password = DEFAULT_PASSWORD,
                CreatedOn = DateTime.Now,
            };

            _mockUserRepository.Setup(x => x.FirstOrDefaultAsync(w => w.Username == user.Username)).ReturnsAsync(user);

            Func<Task> action = async () => await _userService.CreateAsync(user);

            action.Should().Throw<UserAlreadyExistsException>();
        }

        [Fact]
        public void Not_throw_any_exception_when_user_and_balance_for_user_are_created()
        {
            var user = new User()
            {
                Username = DEFAULT_USERNAME,
                Email = DEFAULT_EMAIL,
                Password = DEFAULT_PASSWORD,
                CreatedOn = DateTime.Now,
            };

            User expectedUser = null;

            _mockUserRepository.Setup(x => x.FirstOrDefaultAsync(w => w.Username == DEFAULT_USERNAME)).ReturnsAsync(expectedUser);

            Func<Task> action = async () => await _userService.CreateAsync(user);

            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public async Task Return_user_profile_with_bet_historic_and_deposit_historic()
        {
            var deposits = new List<Deposit>()
            {
                new Deposit()
                {
                    Id = 1,
                    Amount = 10,
                    MerchantId = (int)Merchant.BANK,
                    UserId = DEFAULT_USER_ID
                },
                new Deposit()
                {
                    Id = 2,
                    Amount = 20,
                    MerchantId = (int)Merchant.PAYPAL,
                    UserId = DEFAULT_USER_ID
                }
            };

            var slots = new List<Slot>()
            {
                new Slot()
                {
                    Id = 1,
                    Name = "Slot 1"
                },
                new Slot()
                {
                    Id = 2,
                    Name = "Slot 2"
                }
            };

            var bets = new List<Bet>()
            {
                new Bet()
                {
                    Id = 1,
                    Amount = 0.10,
                    BetType = (int)BetType.NORMAL,
                    CreatedOn = DateTime.Now,
                    SlotId = slots[0].Id,
                    UserId = DEFAULT_USER_ID,
                    WinningAmount = 0,
                    WinningBet = false,
                },
                new Bet()
                {
                    Id = 2,
                    Amount = 0.10,
                    BetType = (int)BetType.NORMAL,
                    CreatedOn = DateTime.Now.AddSeconds(10),
                    SlotId = slots[0].Id,
                    UserId = DEFAULT_USER_ID,
                    WinningAmount = 0.15,
                    WinningBet = true,
                },
                new Bet()
                {
                    Id = 2,
                    Amount = 0.10,
                    BetType = (int)BetType.NORMAL,
                    CreatedOn = DateTime.Now.AddSeconds(20),
                    SlotId = slots[0].Id,
                    UserId = DEFAULT_USER_ID,
                    WinningAmount = 0,
                    WinningBet = false,
                },
                new Bet()
                {
                    Id = 4,
                    Amount = 0.20,
                    BetType = (int)BetType.NORMAL,
                    CreatedOn = DateTime.Now.AddSeconds(30),
                    SlotId = slots[1].Id,
                    UserId = DEFAULT_USER_ID,
                    WinningAmount = 0,
                    WinningBet = false,
                },
                new Bet()
                {
                    Id = 5,
                    Amount = 0.20,
                    BetType = (int)BetType.NORMAL,
                    CreatedOn = DateTime.Now.AddSeconds(40),
                    SlotId = slots[1].Id,
                    UserId = DEFAULT_USER_ID,
                    WinningAmount = 0.25,
                    WinningBet = true,
                },
            };

            var betHistoric = new List<UserBetHistoricResponse>();

            bets.GroupBy(g => g.SlotId).ToList().ForEach((group) =>
            {
                betHistoric.Add(new UserBetHistoricResponse()
                {
                    Bets = bets.Select(s => new BetHistoricResponse()
                    {
                        Id = s.Id,
                        BetAmount = s.Amount,
                        WinningBet = s.WinningBet,
                        WinningAmount = s.WinningAmount,
                        SlotId = s.SlotId,
                        CreatedOn = s.CreatedOn,
                        BetType = s.BetType,
                    }).ToList(),
                    SlotName = slots.FirstOrDefault(w => w.Id == group.Key).Name,
                });
            });

            var depositHistoric = deposits.Select(s => new DepositHistoricResponse()
            {
                Id = s.Id,
                Amount = s.Amount,
                MerchantId = s.MerchantId
            }).ToList();

            var user = new User()
            {
                Id = DEFAULT_USER_ID,
                Email = DEFAULT_EMAIL,
                Username = DEFAULT_USERNAME,
                CreatedOn = DateTime.Now,
            };

            var currentBalance = new BalanceResponse()
            {
                CurrentBalance = 24.33
            };

            var expectedUserProfile = new ProfileResponse()
            {
                Username = DEFAULT_USERNAME,
                Email = DEFAULT_EMAIL,
                BetHistoric = betHistoric,
                DepositHistoric = depositHistoric,
                CurrentBalance = currentBalance,
            };

            _mockTokenService.Setup(x => x.GetCurrentUserId()).Returns(DEFAULT_USER_ID);
            _mockUserRepository.Setup(x => x.GetByIdAsync(DEFAULT_USER_ID)).ReturnsAsync(user);
            _mockBetService.Setup(x => x.GetCurrentUserBetHistoricAsync()).ReturnsAsync(betHistoric);
            _mockDepositService.Setup(x => x.GetDepositsForCurrentUserAsync()).ReturnsAsync(depositHistoric);
            _mockBalanceService.Setup(x => x.GetCurrentUserCurrentBalanceAsync()).ReturnsAsync(currentBalance);

            var result = await _userService.GetCurrentUserProfile();

            result.Should().BeEquivalentTo(expectedUserProfile);
        }
    }
}
