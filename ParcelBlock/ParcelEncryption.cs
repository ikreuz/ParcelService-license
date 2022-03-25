using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ParcelBlock
{
    public class ParcelEncryption
    {
        private const string _defaultPassword = "yA384g%cCr#=-=?#S&2!Z9LT-fPp9B#YSqn";

        public static string Encrypt(string original)
        {
            return Encrypt(original, _defaultPassword);
        }

        public static string Encrypt(string original, string password)
        {
            string encrypted;
            TripleDESCryptoServiceProvider cryptoProvider;
            MD5CryptoServiceProvider md5Hash;
            byte[] passwordHash;

            md5Hash = new MD5CryptoServiceProvider();
            passwordHash = md5Hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            md5Hash = null;
            GC.Collect();

            cryptoProvider = new TripleDESCryptoServiceProvider();
            cryptoProvider.Key = passwordHash;
            cryptoProvider.Mode = CipherMode.ECB;
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes(original);
            encrypted = Convert.ToBase64String(cryptoProvider.CreateEncryptor().TransformFinalBlock(buffer, 0 ,buffer.Length));
            cryptoProvider = null;
            return encrypted;
        }

        public static string Decrypt(string encrypted)
        {
            return Decrypt(encrypted, _defaultPassword);
        }

        public static string Decrypt(string encrypted, string password)
        {
            string decrypted;
            TripleDESCryptoServiceProvider cryptoProvider;
            MD5CryptoServiceProvider md5Hash;
            byte[] passwordHash;
            md5Hash = new MD5CryptoServiceProvider();
            passwordHash = md5Hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            md5Hash = null;
            GC.Collect();

            cryptoProvider = new TripleDESCryptoServiceProvider();
            cryptoProvider.Key = passwordHash;
            cryptoProvider.Mode = CipherMode.ECB;
            byte[] buffer = Convert.FromBase64String(encrypted);
            decrypted = ASCIIEncoding.ASCII.GetString(cryptoProvider.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
            cryptoProvider = null;
            return decrypted;
        }
    }
}
