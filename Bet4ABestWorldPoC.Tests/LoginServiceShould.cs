using System;
using Xunit;
using FluentAssertions;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Responses;
using Moq;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Utilities;
using System.Threading.Tasks;
using Bet4ABestWorldPoC.Services.Request;

namespace Bet4ABestWorldPoC.Services.Tests
{
    public class LoginServiceShould
    {
        private const string DEFAULT_USERNAME = "test";
        private const string DEFAULT_PASSWORD = "passtest";
        private const string DEFAULT_TOKEN = "123456";
        private readonly string DEFAULT_HASHED_PASSWORD = Security.HashPassword("passtest");

        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ITokenService> _mockTokenService;

        private readonly LoginService _loginService;

        public LoginServiceShould()
        {
            _mockUserService = new Mock<IUserService>();
            _mockTokenService = new Mock<ITokenService>();

            _loginService = new LoginService(_mockUserService.Object, _mockTokenService.Object);
        }

        [Fact]
        public void Throw_invalid_login_request_exception_when_request_is_null()
        {
            LoginRequest request = null;

            Func<Task> action = async () => await _loginService.LoginAsync(request);

            action.Should().Throw<InvalidLoginRequestException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throw_invalid_username_exception_when_username_is_empty_or_null(string username)
        {
            var request = new LoginRequest(username, DEFAULT_PASSWORD);

            Func<Task> action = async () => await _loginService.LoginAsync(request);
            
            action.Should().Throw<InvalidUsernameException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throw_invalid_password_exception_when_password_is_empty_or_null(string password)
        {
            var request = new LoginRequest(DEFAULT_USERNAME, password);

            Func<Task> action = async () => await _loginService.LoginAsync(request);

            action.Should().Throw<InvalidPasswordException>();
        }

        [Fact]
        public void Throw_invalid_credentials_exception_when_username_is_invalid()
        {
            var invalidUsername = "teststest";

            var request = new LoginRequest(invalidUsername, DEFAULT_PASSWORD);

            Func<Task> action = async () => await _loginService.LoginAsync(request);

            action.Should().Throw<InvalidCredentialsException>();
        }

        [Fact]
        public void Throw_invalid_credentials_exception_when_password_is_invalid()
        {
            
            var invalidPassword = "passwordTests";

            var request = new LoginRequest(DEFAULT_USERNAME, invalidPassword);

            Func<Task> action = async () => await _loginService.LoginAsync(request);

            action.Should().Throw<InvalidCredentialsException>();
        }

        [Fact]
        public async void Return_valid_login_response_when_username_and_password_are_valid()
        {
            var expectedLoginResponse = new LoginResponse()
            {
                Token = DEFAULT_TOKEN,
            };
            var expectedUser = new Repositories.Entities.User()
            {
                Password = DEFAULT_HASHED_PASSWORD,
                Username = DEFAULT_USERNAME
            };
            _mockUserService.Setup(x => x.GetUserByUsernameAsync(DEFAULT_USERNAME)).ReturnsAsync(expectedUser);

            _mockTokenService.Setup(x => x.GenerateToken(expectedUser)).Returns(DEFAULT_TOKEN);

            var request = new LoginRequest(DEFAULT_USERNAME, DEFAULT_PASSWORD);

            var loginResponse = await _loginService.LoginAsync(request);

            loginResponse.Token.Should().Be(expectedLoginResponse.Token);
        }
    }



}
