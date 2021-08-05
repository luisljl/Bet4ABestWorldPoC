using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Services.Responses;
using Bet4ABestWorldPoC.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services
{
    public class LoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenGenerator _tokenGenerator;
        
        public LoginService(IUserRepository userRepository, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResponse> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new InvalidUsernameException();
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidPasswordException();
            }

            var user = await _userRepository.GetByUsername(username);
            if (user == null || !Security.VerifyPassword(password, user.Password))
            {
                throw new InvalidCredentialsException();
            }

            return new LoginResponse()
            {
                Token = _tokenGenerator.GenerateToken(user),
            };
        }
    }
}
