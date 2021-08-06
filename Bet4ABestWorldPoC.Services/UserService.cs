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
        
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Create(User newUser)
        {
            ValidateUser(newUser);
            var user = await _userRepository.FirstOrDefault(w => w.Username == newUser.Username);
            if (user != null)
            {
                throw new UserAlreadyExistsException();
            }
            await _userRepository.Add(newUser);
        }

        public async Task<User> GetById(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new InvalidUsernameException();
            }
            var user = await _userRepository.FirstOrDefault(w => w.Username == username);
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
