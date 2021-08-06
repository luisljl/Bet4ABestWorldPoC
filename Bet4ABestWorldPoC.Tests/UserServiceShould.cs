using System;
using Xunit;
using FluentAssertions;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Responses;
using Moq;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Utilities;
using System.Threading.Tasks;
using Bet4ABestWorldPoC.Repositories.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Bet4ABestWorldPoC.Services.Tests
{
    public class UserServiceShould
    {
        private const string DEFAULT_USERNAME = "test";
        private const string DEFAULT_EMAIL = "test@test.com";
        private const string DEFAULT_PASSWORD = "passtest";

        private readonly Mock<IUserRepository> _mockUserRepository;

        private readonly UserService _userService;

        public UserServiceShould()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
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
        public void Not_throw_any_exception_when_user_is_created()
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
    }
}
