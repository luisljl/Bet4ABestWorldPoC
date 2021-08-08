namespace Bet4ABestWorldPoC.Services.Request
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public RegisterRequest(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
