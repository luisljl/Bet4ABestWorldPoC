using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using Bet4ABestWorldPoC.Shared.Settings;
using Moq;
using Microsoft.Extensions.Options;
using Bet4ABestWorldPoC.Repositories.Entities;
using System;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Bet4ABestWorldPoC.Services.Tests
{
    public class TokenServiceShould
    {
        private const int DEFAULT_ID = 1;
        private const string DEFAULT_EMAIL = "test@email.test";
        private const string DEFAULT_USERNAME = "John Doe";
        private const string DEFAULT_JWT_SECRET = "F)J@NcRfUjWnZr4u7x!A%D*G-KaPdSgV";
        private const string DEFAULT_TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.Nkuy9Br6gKbhxZOEUehjSxOIVING6pXplakVz9rATwg";

        private readonly User DEFAULT_USER = new()
        {
            Id = DEFAULT_ID,
            Username = DEFAULT_USERNAME,
            Email = DEFAULT_EMAIL,
            Password = "test",
            CreatedOn = DateTime.Now,
        };

        private readonly ClaimsIdentity DEFAULT_CLAIMS = new();

        private readonly IOptions<AppSettings> _appSettings;
        private readonly Mock<IBlackListTokenRepository> _mockBlackListTokenRepository;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly TokenService _tokenService;


        public TokenServiceShould()
        {
            _appSettings = Options.Create(new AppSettings()
            {
                JWTSecret = DEFAULT_JWT_SECRET
            });

            _mockBlackListTokenRepository = new Mock<IBlackListTokenRepository>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            DEFAULT_CLAIMS.AddClaim(new Claim("nameid", DEFAULT_ID.ToString()));
            DEFAULT_CLAIMS.AddClaim(new Claim("name", DEFAULT_USERNAME.ToString()));
            DEFAULT_CLAIMS.AddClaim(new Claim("email", DEFAULT_EMAIL));

            _tokenService = new TokenService(_appSettings, _mockBlackListTokenRepository.Object, _mockHttpContextAccessor.Object);
        }

        [Fact]
        public void Return_a_string_when_a_token_is_generated()
        {
            var result = _tokenService.GenerateToken(DEFAULT_USER);

            result.Should().BeOfType<string>();
        }

        [Fact]
        public void Not_throw_a_exception_when_a_token_is_generated()
        {
            Action action = () => _tokenService.GenerateToken(DEFAULT_USER);

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
            _mockHttpContextAccessor.Setup(x => x.HttpContext.Request.Headers["Authorization"]).Returns(DEFAULT_TOKEN);

            Func<Task> action = async () => await _tokenService.InvalidateTokenAsync(DEFAULT_ID);

            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void Not_throw_a_exception_when_a_blacklist_token_does_not_exists_and_want_to_delete()
        {
            _mockBlackListTokenRepository.Setup(x => x.FirstOrDefaultAsync(w => w.UserId == DEFAULT_ID)).ReturnsAsync(null as BlackListToken);

            Func<Task> action = async () => await _tokenService.DeleteInvalidTokenAsync(DEFAULT_ID);

            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void Not_throw_a_exception_when_a_blacklist_token_is_deleted()
        {
            Func<Task> action = async () => await _tokenService.DeleteInvalidTokenAsync(DEFAULT_ID);

            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public async void Return_null_when_token_does_not_exists()
        {
            _mockBlackListTokenRepository.Setup(x => x.FirstOrDefaultAsync(w => w.InvalidToken == DEFAULT_TOKEN)).ReturnsAsync(null as BlackListToken);

            var result = await _tokenService.GetInvalidTokenAsyncByUserIdAsync(DEFAULT_ID);

            result.Should().BeNull();
        }

        [Fact]
        public async void Return_invalid_token_when_token_is_in_db()
        {
            var expectedInvalidToken = new BlackListToken()
            {
                CreatedOn = DateTime.Now,
                InvalidToken = DEFAULT_TOKEN,
                UserId = DEFAULT_ID,
            };

            _mockBlackListTokenRepository.Setup(x => x.FirstOrDefaultAsync(w => w.UserId == DEFAULT_ID)).ReturnsAsync(expectedInvalidToken);

            var result = await _tokenService.GetInvalidTokenAsyncByUserIdAsync(1);

            result.Should().Be(expectedInvalidToken);
        }

        [Fact]
        public void Return_null_if_token_is_not_in_request()
        {

            _mockHttpContextAccessor.Setup(x => x.HttpContext.Request.Headers["Authorization"]).Returns(null as string);

            var result = _tokenService.GetCurrentUserToken();

            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Return_token_if_token_is_in_request()
        {
            _mockHttpContextAccessor.Setup(x => x.HttpContext.Request.Headers["Authorization"]).Returns(DEFAULT_TOKEN);

            var result = _tokenService.GetCurrentUserToken();

            result.Should().Be(DEFAULT_TOKEN);
        }

        [Fact]
        public void Return_invalid_token_exception_if_token_is_not_in_request_when_search_user_id()
        {
            _mockHttpContextAccessor.Setup(x => x.HttpContext.Request.Headers["Authorization"]).Returns(null as string);

            Action action = () => _tokenService.GetCurrentUserId();

            action.Should().Throw<InvalidTokenException>();
        }

        [Fact]
        public void Return_username_from_current_user()
        {
            _mockHttpContextAccessor.Setup(x => x.HttpContext.Request.Headers["Authorization"]).Returns(DEFAULT_TOKEN);
            _mockHttpContextAccessor.Setup(x => x.HttpContext.User.Identity).Returns(DEFAULT_CLAIMS);

            var result = _tokenService.GetCurrentUserUsername();

            result.Should().Be(DEFAULT_USERNAME);
        }

        [Fact]
        public void Return_invalid_token_exception_if_token_is_not_in_request_when_search_user_username()
        {
            _mockHttpContextAccessor.Setup(x => x.HttpContext.Request.Headers["Authorization"]).Returns(null as string);

            Action action = () => _tokenService.GetCurrentUserUsername();

            action.Should().Throw<InvalidTokenException>();
        }

        [Fact]
        public void Return_invalid_token_exception_if_token_is_not_in_request_when_search_user_email()
        {
            _mockHttpContextAccessor.Setup(x => x.HttpContext.Request.Headers["Authorization"]).Returns(null as string);

            Action action = () => _tokenService.GetCurrentUserEmail();

            action.Should().Throw<InvalidTokenException>();
        }
    }
}
