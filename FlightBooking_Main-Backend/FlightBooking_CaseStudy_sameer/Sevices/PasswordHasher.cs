
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Security.Policy;

namespace FlightBooking_CaseStudy_sameer.Sevices
{
  public class PasswordHasher
        {
            // Hashes the password and returns the combined salt and hash
            public string HashPassword(string password)
            {
                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentException("Password cannot be null or empty.", nameof(password));
                }
                // Generate a random salt
                var salt = new byte[16];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
                var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                  password: password,
                  salt: salt,
                  prf: KeyDerivationPrf.HMACSHA256,
                  iterationCount: 10000,
                  numBytesRequested: 32));
                // Combine salt and hash
                return $"{Convert.ToBase64String(salt)}.{hash}";


            }
        // Verifies if the provided password matches the stored hashed password
        public bool VerifyPassword(string hashedPassword, string password)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            var parts = hashedPassword.Split('.');
            if (parts.Length != 2)
            {
                return false;
            }
            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = parts[1];
            // Compute the hash of the provided password using the stored salt
            var computedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));

            // Compare the stored hash with the computed hash
            return storedHash == computedHash;
           
        }
    }

    }
    
