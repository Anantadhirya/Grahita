using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Grahita.components
{
    public class HashHelper
    {
        public static string HashPassword(string password)
        {
            using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                hasher.DegreeOfParallelism = 8;
                hasher.MemorySize = 65536;
                hasher.Iterations = 4;
                byte[] hashBytes = hasher.GetBytes(32);
                return Convert.ToBase64String(hashBytes);
            }
        }
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return HashPassword(password) == hashedPassword;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
