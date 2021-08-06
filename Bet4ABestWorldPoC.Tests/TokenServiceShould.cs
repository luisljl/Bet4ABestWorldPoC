using Xunit;
using FluentAssertions;
using Bet4ABestWorldPoC.Services.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bet4ABestWorldPoC.Shared.Settings;
using Moq;
using Microsoft.Extensions.Options;
using Bet4ABestWorldPoC.Repositories.Entities;
using System;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Repositories.Interfaces;

namespace Bet4ABestWorldPoC.Services.Tests
{
    public class TokenServiceShould
    {
        private const string DEFAULT_JWT_SECRET = "F)J@NcRfUjWnZr4u7x!A%D*G-KaPdSgV";
        private const string DEFAULT_TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.Nkuy9Br6gKbhxZOEUehjSxOIVING6pXplakVz9rATwg";

        private readonly User DefaultUser = new()
        {
            Id = 1,
            Username = "userTest",
            Email = "test@email.test",
            Password = "test",
            CreatedOn = System.DateTime.Now,
        };

        private readonly IOptions<AppSettings> _appSettings;
        private readonly Mock<IBlackListTokenRepository> _mockBlackListTokenRepository;

        private readonly TokenService _tokenService;


        public TokenServiceShould()
        {
            _appSettings = Options.Create(new AppSettings()
            { 
                JWTSecret = DEFAULT_JWT_SECRET
            });

            _mockBlackListTokenRepository = new Mock<IBlackListTokenRepository>();

            _tokenService = new TokenService(_appSettings, _mockBlackListTokenRepository.Object);
        }

        [Fact]
        public void Return_a_string_when_a_token_is_generated()
        {
            var result = _tokenService.GenerateToken(DefaultUser);

            result.Should().BeOfType<string>();
        }

        [Fact]
        public void Not_throw_a_exception_when_a_token_is_generated()
        {
            Action action = () => _tokenService.GenerateToken(DefaultUser);

            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void Throw_a_invalid_user_data_exception_when_user_is_null()
        {
            Action action = () => _tokenService.GenerateToken(null);

            action.Should().Throw<InvalidUserDataException>();
        }

        [Fact]
        public void Not_throw_a_exception_when_a_blacklist_token_is_created()
        {
            Func<Task> action = async () => await _tokenService.InvalidateToken(DEFAULT_TOKEN);

            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void Throw_a_invalid_token_exception_when_token_is_null_or_empty_when_is_added_to_a_blacklist()
        {
            Func<Task> action = async () => await _tokenService.InvalidateToken(null);

            action.Should().Throw<InvalidTokenException>();
        }

        [Fact]
        public void Throw_a_invalid_token_exception_when_token_is_null_or_empty_when_is_searched()
        {
            Func<Task> action = async () => await _tokenService.GetInvalidToken(null);

            action.Should().Throw<InvalidTokenException>();
        }

        [Fact]
        public async void Return_null_when_token_does_not_exists()
        {
            _mockBlackListTokenRepository.Setup(x => x.Get(DEFAULT_TOKEN)).ReturnsAsync(null as BlackListToken);

            var result = await _tokenService.GetInvalidToken(DEFAULT_TOKEN);

            result.Should().BeNull();
        }

        [Fact]
        public async void Return_invalid_token_when_token_is_in_db()
        {
            var expectedInvalidToken = new BlackListToken()
            {
                CreatedOn = DateTime.Now,
                InvalidToken = DEFAULT_TOKEN,
            };

            _mockBlackListTokenRepository.Setup(x => x.Get(DEFAULT_TOKEN)).ReturnsAsync(expectedInvalidToken);

            var result = await _tokenService.GetInvalidToken(DEFAULT_TOKEN);

            result.Should().Be(expectedInvalidToken);
        }
    }
}
