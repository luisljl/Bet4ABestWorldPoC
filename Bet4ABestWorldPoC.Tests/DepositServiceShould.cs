using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Enums;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Services.Request;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Bet4ABestWorldPoC.Services.Tests
{
    public class DepositServiceShould
    {
        private const int DEFAULT_ID = 1;

        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IDepositRepository> _mockDepositRepository;

        private readonly DepositService _depositService;

        public DepositServiceShould()
        {
            _mockTokenService = new Mock<ITokenService>();
            _mockDepositRepository = new Mock<IDepositRepository>();

            _depositService = new DepositService(_mockTokenService.Object, _mockDepositRepository.Object);
        }

        [Fact]
        public void Throw_minimum_amount_exception_if_amount_is_less_than_10()
        {
            var request = new DepositRequest(5, (int)Merchant.PAYPAL);

            Func<Task> action = async () => await _depositService.DepositAsync(request);

            action.Should().Throw<MinimumAmountException>();
        }


        [Fact]
        public void Throw_invalid_merchant_if_merchant_is_not_valid()
        {
            var inventedMerchantId = 3;

            var request = new DepositRequest(15, inventedMerchantId);

            Func<Task> action = async () => await _depositService.DepositAsync(request);

            action.Should().Throw<InvalidMerchantException>();
        }

        [Fact]
        public void Not_throw_any_exception_when_deposit_is_created()
        {
            var request = new DepositRequest(20, (int)Merchant.PAYPAL);
            var user = new User()
            {
                Id = DEFAULT_ID,
                Email = "test@test.com",
                Username = "test",
                CreatedOn = DateTime.Now,
            };

            _mockTokenService.Setup(x => x.GetCurrentUserId()).Returns(DEFAULT_ID);

            Func<Task> action = async () => await _depositService.DepositAsync(request);

            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void Throw_invalid_deposit_data_exception_if_request_is_null()
        {
            Func<Task> action = async () => await _depositService.DepositAsync(null);

            action.Should().Throw<InvalidDepositDataException>();
        }

        [Theory]
        [InlineData(DEFAULT_ID)]
        [InlineData(2)]
        public async void Return_a_list_of_deposits_by_user(int userId)
        {
            var deposits = new List<Deposit>()
            {
                new Deposit()
                {
                    Id = 1,
                    UserId = DEFAULT_ID,
                    Amount = 20,
                    MerchantId = (int)Merchant.BANK
                },
                new Deposit()
                {
                    Id = 2,
                    UserId = 2,
                    Amount = 30,
                    MerchantId = (int)Merchant.BANK
                },
                new Deposit()
                {
                    Id = 3,
                    UserId = DEFAULT_ID,
                    Amount = 40,
                    MerchantId = (int)Merchant.PAYPAL
                },
                new Deposit()
                {
                    Id = 4,
                    UserId = DEFAULT_ID,
                    Amount = 40,
                    MerchantId = (int)Merchant.BANK
                },
                new Deposit()
                {
                    Id = 5,
                    UserId = 2,
                    Amount = 40,
                    MerchantId = (int)Merchant.PAYPAL
                }
            };

            var expectedDeposit = deposits.Where(w => w.UserId == userId).ToList();

            _mockDepositRepository.Setup(x => x.GetAllWhereAsync(w => w.UserId == userId)).ReturnsAsync(expectedDeposit);

            var result = await _depositService.GetDepositsForUserAsync(userId);

            result.Should().HaveCount(expectedDeposit.Count);
        }

    }
}
