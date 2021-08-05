using System;
using Xunit;
using FluentAssertions;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Responses;
using Moq;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Utilities;
using Bet4ABestWorldPoC.Services.Request;
using Bet4ABestWorldPoC.Repositories.Entities;

namespace Bet4ABestWorldPoC.Services.Tests
{
    public class RegisterServiceShould
    {
        private const string DEFAULT_USERNAME = "test";
        private const string DEFAULT_EMAIL = "test@test.com";
        private const string DEFAULT_PASSWORD = "passtest";

        private readonly Mock<IUserRepository> _mockUserRepository;

        private readonly RegisterService _registerService;

        public RegisterServiceShould()
        {
            _mockUserRepository = new Mock<IUserRepository>();

            _registerService = new RegisterService(_mockUserRepository.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throw_invalid_username_exception_when_username_is_empty_or_null(string username)
        {
            var request = new RegisterRequest(username, DEFAULT_PASSWORD, DEFAULT_EMAIL);

            Action action = () => _registerService.Register(request);
            
            action.Should().Throw<InvalidUsernameException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throw_invalid_password_exception_when_password_is_empty_or_null(string password)
        {
            var request = new RegisterRequest(DEFAULT_USERNAME, password, DEFAULT_EMAIL);

            Action action = () => _registerService.Register(request);

            action.Should().Throw<InvalidPasswordException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Throw_invalid_email_exception_when_email_is_empty_or_null(string email)
        {
            var request = new RegisterRequest(DEFAULT_USERNAME, DEFAULT_PASSWORD, email);

            Action action = () => _registerService.Register(request);

            action.Should().Throw<InvalidEmailException>();
        }

        [Fact]
        public void Throw_user_alredy_exists_exception_when_username_exists_in_db()
        {
            var request = new RegisterRequest(DEFAULT_USERNAME, DEFAULT_PASSWORD, DEFAULT_EMAIL);

            var expectedUser = new Repositories.Entities.User()
            {
                Username = DEFAULT_USERNAME
            };

            _mockUserRepository.Setup(x => x.GetByUsername(DEFAULT_USERNAME)).Returns(expectedUser);

            Action action = () => _registerService.Register(request);

            action.Should().Throw<UserAlreadyExistsException>();
        }

        [Fact]
        public void Not_throw_any_exception_when_register_is_valid()
        {
            var request = new RegisterRequest(DEFAULT_USERNAME, DEFAULT_PASSWORD, DEFAULT_EMAIL);

            User expectedUser = null;

            _mockUserRepository.Setup(x => x.GetByUsername(DEFAULT_USERNAME)).Returns(expectedUser);

            Action action = () => _registerService.Register(request);

            action.Should().NotThrow<Exception>();
        }
    }



}
