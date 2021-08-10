using System.ComponentModel.DataAnnotations;

namespace Bet4ABestWorldPoC.Services.Request
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }

        public RegisterRequest(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
