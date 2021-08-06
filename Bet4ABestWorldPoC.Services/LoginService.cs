using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Services.Request;
using Bet4ABestWorldPoC.Services.Responses;
using Bet4ABestWorldPoC.Utilities;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        
        public LoginService(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            ValidateLoginRequest(request);
            var user = await _userService.GetUserByUsernameAsync(request.Username);
            if (user == null || !Security.VerifyPassword(request.Password, user.Password))
            {
                throw new InvalidCredentialsException();
            }

            return new LoginResponse()
            {
                Token = _tokenService.GenerateToken(user),
            };
        }

        private void ValidateLoginRequest(LoginRequest request)
        {
            if (request == null)
            {
                throw new InvalidLoginRequestException();
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
    }
}
