﻿using System.Security.Cryptography;
using System.Text;

namespace ClinicService
{
    public static class PasswordUtils
    {
        private const string SecretKey = "Fz8wMguqN2DGWiD1ICvRxQ==";

        public static (string passwordSalt, string passwordHash) CreatePasswordHash(string password)
        {
            // generate random salt 
            var buffer = new byte[16];
            RNGCryptoServiceProvider secureRandom = new();
            secureRandom.GetBytes(buffer);

            // create hash 
            var passwordSalt = Convert.ToBase64String(buffer);
            var passwordHash = GetPasswordHash(password, passwordSalt);

            // done
            return (passwordSalt, passwordHash);
        }

        public static bool VerifyPassword(string password, string passwordSalt,
            string passwordHash)
        {
            return GetPasswordHash(password, passwordSalt) == passwordHash;
        }

        public static string GetPasswordHash(string password, string passwordSalt)
        {
            // build password string
            password = $"{password}~{passwordSalt}~{SecretKey}";
            var buffer = Encoding.UTF8.GetBytes(password);

            // compute hash 
            SHA512 sha512 = new SHA512Managed();
            var passwordHash = sha512.ComputeHash(buffer);

            // done
            return Convert.ToBase64String(passwordHash);
        }
    }
}
