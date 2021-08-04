using System;

namespace Bet4ABestWorldPoC.Utilities
{
    public class Security
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        public static bool VerifyPassword(string password, string originalPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, originalPassword);
        }
    }
}
