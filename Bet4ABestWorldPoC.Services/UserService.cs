using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Services.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBalanceService _balanceService;
        private readonly ITokenService _tokenService;
        private readonly IDepositService _depositService;
        private readonly IBetService _BetService;

        public UserService(IUserRepository userRepository, IBalanceService balanceService, ITokenService tokenService, IBetService betService, IDepositService depositService)
        {
            _userRepository = userRepository;
            _balanceService = balanceService;
            _tokenService = tokenService;
            _BetService = betService;
            _depositService = depositService;
        }

        public async Task CreateAsync(User newUser)
        {
            ValidateUser(newUser);
            var user = await _userRepository.FirstOrDefaultAsync(w => w.Username == newUser.Username);
            if (user != null)
            {
                throw new UserAlreadyExistsException();
            }
            await _userRepository.CreateAsync(newUser);

            await _balanceService.CreateAsync(newUser.Id);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        public async Task<ProfileResponse> GetCurrentUserProfile()
        {
            var currentUserId = _tokenService.GetCurrentUserId();
            var currentUser = await GetByIdAsync(currentUserId);
            var betHistoric = await _BetService.GetCurrentUserBetHistoricAsync();
            var depositHistoric = await _depositService.GetDepositsForCurrentUserAsync();
            var currentBalance = await _balanceService.GetCurrentUserCurrentBalanceAsync();

            var response = new ProfileResponse()
            {
                Email = currentUser.Email,
                Username = currentUser.Username,
                BetHistoric = betHistoric ?? new List<UserBetHistoricResponse>(),
                DepositHistoric = depositHistoric ?? new List<DepositHistoricResponse>(),
                CurrentBalance = currentBalance,
            };

            return response;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new InvalidUsernameException();
            }
            var user = await _userRepository.FirstOrDefaultAsync(w => w.Username == username);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        private void ValidateUser(User user)
        {
            if (user == null)
            {
                throw new InvalidUserDataException();
            }
            if (string.IsNullOrWhiteSpace(user.Username))
            {
                throw new InvalidUsernameException();
            }
            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new InvalidPasswordException();
            }
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new InvalidEmailException();
            }
        }
    }
}
