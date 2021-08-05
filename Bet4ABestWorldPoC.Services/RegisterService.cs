using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Request;
using Bet4ABestWorldPoC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services
{
    public class RegisterService
    {
        private readonly IUserRepository _userRepository;

        public RegisterService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Register(RegisterRequest request)
        {
            ValidateRegisterRequest(request);
            var user = _userRepository.GetByUsername(request.Username);
            if (user != null)
            {
                throw new UserAlreadyExistsException();
            }
            var newUser = MapNewUserFromRequest(request);
            _userRepository.Save(newUser);
        }

        private void ValidateRegisterRequest(RegisterRequest request)
        {
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
