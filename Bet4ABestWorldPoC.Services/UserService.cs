using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBalanceService _balanceService;
        
        public UserService(IUserRepository userRepository, IBalanceService balanceService)
        {
            _userRepository = userRepository;
            _balanceService = balanceService;
        }

        public async Task CreateAsync(User newUser)
        {
            ValidateUser(newUser);
            var user = await _userRepository.FirstOrDefaultAsync(w => w.Username == newUser.Username);
            if (user != null)
            {
                throw new UserAlreadyExistsException();
            }
            await _userRepository.AddAsync(newUser);

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
