using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Bet4ABestWorldPoC.Services.Tests
{
    public class BalanceServiceShould
    {
        private const int DEFAULT_USER_ID = 1;

        private readonly Mock<IBalanceRepository> _mockBalanceRepository;
        private readonly Mock<ITokenService> _mockTokenService;

        private readonly BalanceService _balanceService;

        public BalanceServiceShould()
        {
            _mockBalanceRepository = new Mock<IBalanceRepository>();
            _mockTokenService = new Mock<ITokenService>();

            _balanceService = new BalanceService(_mockBalanceRepository.Object, _mockTokenService.Object);
        }

        [Fact]
        public void Throw_invalid_balance_data_exception_if_amount_is_negative_when_increase_balance()
        {
            _mockTokenService.Setup(x => x.GetCurrentUserId()).Returns(DEFAULT_USER_ID);

            Func<Task> action = async () => await _balanceService.IncreaseBalanceAsync(-100);

            action.Should().Throw<InvalidBalanceDataException>();
        }

        [Fact]
        public void Throw__balance_not_found_exception_if_balance_does_not_exists_when_increase_balance()
        {
            _mockTokenService.Setup(x => x.GetCurrentUserId()).Returns(DEFAULT_USER_ID);

            Func<Task> action = async () => await _balanceService.IncreaseBalanceAsync(50);

            action.Should().Throw<BalanceNotFoundException>();
        }

        [Fact]
        public void Throw_invalid_balance_data_exception_if_amount_is_negative_when_decrease_balance()
        {
            _mockTokenService.Setup(x => x.GetCurrentUserId()).Returns(DEFAULT_USER_ID);

            Func<Task> action = async () => await _balanceService.DecreaseBalanceAsync(-100);

            action.Should().Throw<InvalidBalanceDataException>();
        }

        [Fact]
        public void Throw__balance_not_found_exception_if_balance_does_not_exists_when_decrease_balance()
        {
            _mockTokenService.Setup(x => x.GetCurrentUserId()).Returns(DEFAULT_USER_ID);

            Func<Task> action = async () => await _balanceService.DecreaseBalanceAsync(50);

            action.Should().Throw<BalanceNotFoundException>();
        }

        [Fact]
        public void Not_throw_any_exception_when_balance_is_increased_and_valid()
        {
            _mockTokenService.Setup(x => x.GetCurrentUserId()).Returns(DEFAULT_USER_ID);

            var balance = new Balance()
            {
                Id = 1,
                UserId = DEFAULT_USER_ID,
                Amount = 10,
                LastUpdate = DateTime.Now
            };

            _mockBalanceRepository.Setup(x => x.FirstOrDefaultAsync(w => w.UserId == DEFAULT_USER_ID)).ReturnsAsync(balance);

            Func<Task> action = async () => await _balanceService.IncreaseBalanceAsync(0.15);

            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void Not_throw_any_exception_when_balance_is_decreased_and_valid()
        {
            _mockTokenService.Setup(x => x.GetCurrentUserId()).Returns(DEFAULT_USER_ID);

            var balance = new Balance()
            {
                Id = 1,
                UserId = DEFAULT_USER_ID,
                Amount = 10,
                LastUpdate = DateTime.Now
            };

            _mockBalanceRepository.Setup(x => x.FirstOrDefaultAsync(w => w.UserId == DEFAULT_USER_ID)).ReturnsAsync(balance);

            Func<Task> action = async () => await _balanceService.DecreaseBalanceAsync(0.15);

            action.Should().NotThrow<Exception>();
        }

        [Theory]
        [InlineData(24.3)]
        [InlineData(11.25)]
        public async Task Return_user_current_balance(double expectedAmount)
        {
            var expectedBalance = new Balance()
            {
                Amount = expectedAmount,
                UserId = DEFAULT_USER_ID
            };

            _mockBalanceRepository.Setup(x => x.FirstOrDefaultAsync(w => w.UserId == DEFAULT_USER_ID)).ReturnsAsync(expectedBalance);

            var result = await _balanceService.GetUserCurrentBalanceAsync(DEFAULT_USER_ID);

            result.CurrentBalance.Should().Be(expectedAmount);
        }
    }
}
