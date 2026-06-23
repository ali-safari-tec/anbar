using System;
using System.Text;
using Konscious.Security.Cryptography;

namespace anbar.Tools
{
    internal class HashPassword
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 4;
        private const int MemorySize = 1024 * 64;
        private const int Parallelism = 8;

        public static string PasswordHash(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = Parallelism,
                Iterations = Iterations,
                MemorySize = MemorySize
            };

            byte[] hashBytes = argon2.GetBytes(HashSize);

            var hashWithSalt = new byte[salt.Length + hashBytes.Length];
            Buffer.BlockCopy(salt, 0, hashWithSalt, 0, salt.Length);
            Buffer.BlockCopy(hashBytes, 0, hashWithSalt, salt.Length, hashBytes.Length);

            return Convert.ToBase64String(hashWithSalt);
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
                return false;

            byte[] hashWithSalt;
            try
            {
                hashWithSalt = Convert.FromBase64String(storedHash);
            }
            catch (FormatException)
            {
                return false;
            }

            if (hashWithSalt.Length < SaltSize + HashSize)
                return false;


            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashWithSalt, 0, salt, 0, salt.Length);

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = Parallelism,
                Iterations = Iterations,
                MemorySize = MemorySize
            };

            byte[] computedHash = argon2.GetBytes(HashSize);

            return CompareByteArrays(hashWithSalt, computedHash, salt.Length);
        }

        private static bool CompareByteArrays(byte[] originalHash, byte[] computedHash, int offset)
        {
            int hashLength = computedHash.Length;
            bool isMatch = true;

            for (int i = 0; i < hashLength; i++)
            {
                if (originalHash[i + offset] != computedHash[i])
                {
                    isMatch = false;
                }
            }
            return isMatch;
        }
    }
}
