using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Services.Request;
using Bet4ABestWorldPoC.Utilities;
using System;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserService _userService;

        public RegisterService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task RegisterUserAsync(RegisterRequest request)
        {
            ValidateRegisterRequest(request);
            var newUser = MapNewUserFromRequest(request);
            await _userService.CreateAsync(newUser);
        }

        private void ValidateRegisterRequest(RegisterRequest request)
        {
            if (request == null)
            {
                throw new InvalidRegisterRequestException();
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new InvalidEmailException();
            }

            if (string.IsNullOrWhiteSpace(request.Username))
            {
                throw new InvalidUsernameException();
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new InvalidPasswordException();
            }
        }

        private User MapNewUserFromRequest(RegisterRequest request)
        {
            return new User()
            {
                Username = request.Username,
                Password = Security.HashPassword(request.Password),
                Email = request.Email,
                CreatedOn = DateTime.Now
            };
        }
    }
}
