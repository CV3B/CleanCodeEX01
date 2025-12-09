using Inlämningsuppgift_1.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Inlämningsuppgift_1.Core.Services
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ValidationException("Password cannot be null or empty.");

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword) || string.IsNullOrWhiteSpace(providedPassword))
                throw new ValidationException("Hashed password and provided password cannot be null or empty.");

            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }
}
