using System;
using System.Security.Cryptography;
using System.Text;

namespace Fanda.Core
{
    public static class PasswordStorage
    {
        public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            if (password == null)
            {
                throw new BadRequestException("Password is required");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new BadRequestException("Password cannot be empty or whitespace only string.");
            }

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        public static bool VerifyPasswordHash(string password, string base64Hash, string base64Salt)
        {
            if (password == null)
            {
                throw new BadRequestException("Password is required");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new BadRequestException("Password cannot be empty or whitespace only string");
            }

            var storedHash = Convert.FromBase64String(base64Hash);
            var storedSalt = Convert.FromBase64String(base64Salt);

            if (storedHash.Length != 64)
            {
                throw new BadRequestException("Invalid length of password hash (64 bytes expected)");
            }

            if (storedSalt.Length != 128)
            {
                throw new BadRequestException("Invalid length of password salt (128 bytes expected).");
            }

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}