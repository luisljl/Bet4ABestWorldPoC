using System;
using Xunit;
using FluentAssertions;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Responses;
using Moq;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Utilities;

namespace Bet4ABestWorldPoC.Services.Tests
{
    public class LoginServiceShould
    {
        private const string DEFAULT_USERNAME = "test";
        private const string DEFAULT_PASSWORD = "passtest";
        private const string DEFAULT_TOKEN = "123456";
        private readonly string DEFAULT_HASHED_PASSWORD = Security.HashPassword("passtest");

        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ITokenGenerator> _mockTokenGenerator;

        private readonly LoginService _loginService;

        public LoginServiceShould()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockTokenGenerator = new Mock<ITokenGenerator>();

            _loginService = new LoginService(_mockUserRepository.Object, _mockTokenGenerator.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throw_invalid_username_exception_when_username_is_empty_or_null(string username)
        {
            Action action = () => _loginService.Login(username, DEFAULT_PASSWORD);
            
            action.Should().Throw<InvalidUsernameException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throw_invalid_password_exception_when_password_is_empty_or_null(string password)
        {
            Action action = () => _loginService.Login(DEFAULT_USERNAME, password);

            action.Should().Throw<InvalidPasswordException>();
        }

        [Fact]
        public void Throw_invalid_credentials_exception_when_username_is_invalid()
        {
            var invalidUsername = "teststest";

            Action action = () => _loginService.Login(invalidUsername, DEFAULT_PASSWORD);

            action.Should().Throw<InvalidCredentialsException>();
        }

        [Fact]
        public void Throw_invalid_credentials_exception_when_password_is_invalid()
        {
            
            var invalidPassword = "passwordTests";

            Action action = () => _loginService.Login(DEFAULT_USERNAME, invalidPassword);

            action.Should().Throw<InvalidCredentialsException>();
        }

        [Fact]
        public void Return_valid_login_response_when_username_and_password_are_valid()
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
            _mockUserRepository.Setup(x => x.GetByUsername(DEFAULT_USERNAME)).Returns(expectedUser);

            _mockTokenGenerator.Setup(x => x.GenerateToken(expectedUser)).Returns(DEFAULT_TOKEN);

            var loginResponse = _loginService.Login(DEFAULT_USERNAME, DEFAULT_PASSWORD);

            loginResponse.Token.Should().Be(expectedLoginResponse.Token);
        }
    }



}
