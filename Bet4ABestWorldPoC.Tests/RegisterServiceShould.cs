using System;
using Xunit;
using FluentAssertions;
using Bet4ABestWorldPoC.Services.Exceptions;
using Moq;
using Bet4ABestWorldPoC.Services.Request;
using Bet4ABestWorldPoC.Repositories.Entities;
using System.Threading.Tasks;
using Bet4ABestWorldPoC.Services.Interfaces;

namespace Bet4ABestWorldPoC.Services.Tests
{
    public class RegisterServiceShould
    {
        private const string DEFAULT_USERNAME = "test";
        private const string DEFAULT_EMAIL = "test@test.com";
        private const string DEFAULT_PASSWORD = "passtest";

        private readonly Mock<IUserService> _mockUserService;

        private readonly RegisterService _registerService;

        public RegisterServiceShould()
        {
            _mockUserService = new Mock<IUserService>();

            _registerService = new RegisterService(_mockUserService.Object);
        }

        [Fact]
        public void Throw_invalid_register_request_exception_when_request_is_null()
        {
            RegisterRequest request = null;

            Func<Task> action = async () => await _registerService.RegisterUserAsync(request);

            action.Should().Throw<InvalidRegisterRequestException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throw_invalid_username_exception_when_username_is_empty_or_null(string username)
        {
            var request = new RegisterRequest(username, DEFAULT_PASSWORD, DEFAULT_EMAIL);

            Func<Task> action = async () => await _registerService.RegisterUserAsync(request);
            
            action.Should().Throw<InvalidUsernameException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throw_invalid_password_exception_when_password_is_empty_or_null(string password)
        {
            var request = new RegisterRequest(DEFAULT_USERNAME, password, DEFAULT_EMAIL);

            Func<Task> action = async () => await _registerService.RegisterUserAsync(request);

            action.Should().Throw<InvalidPasswordException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throw_invalid_email_exception_when_email_is_empty_or_null(string email)
        {
            var request = new RegisterRequest(DEFAULT_USERNAME, DEFAULT_PASSWORD, email);

            Func<Task> action = async () => await _registerService.RegisterUserAsync(request);

            action.Should().Throw<InvalidEmailException>();
        }

        [Fact]
        public void Not_throw_any_exception_when_register_is_valid()
        {
            var request = new RegisterRequest(DEFAULT_USERNAME, DEFAULT_PASSWORD, DEFAULT_EMAIL);

            User expectedUser = null;

            _mockUserService.Setup(x => x.GetUserByUsernameAsync(DEFAULT_USERNAME)).ReturnsAsync(expectedUser);

            Func<Task> action = async () => await _registerService.RegisterUserAsync(request);

            action.Should().NotThrow<Exception>();
        }
    }
}
