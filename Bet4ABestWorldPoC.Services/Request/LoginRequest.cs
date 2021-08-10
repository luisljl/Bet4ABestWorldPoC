using System.ComponentModel.DataAnnotations;

namespace Bet4ABestWorldPoC.Services.Request
{
    public class LoginRequest
    {

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public LoginRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
