using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace EntertainmentAPI.Utilities
{
    public static class PasswordHelper
    {
        // Method to hash a password
        public static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[128 / 8]; // 128 bit salt
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password with the salt using PBKDF2 and HMACSHA256
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Combine salt and hash to save both in the database
            return $"{Convert.ToBase64String(salt)}.{hashed}"; // Format: salt.hash
        }

        // Method to verify the entered password against the stored hash
        public static bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            // Split the stored password into salt and hash
            var parts = storedPassword.Split('.');
            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = parts[1];

            // Hash the entered password with the extracted salt
            string enteredHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Compare the hashed password to the stored hash
            return enteredHash == storedHash;
        }
    }
}
