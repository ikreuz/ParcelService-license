using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ParcelBlock
{
    public class ParcelTools
    {
        public static string GetBlockchain(string block, bool isCompleted)
        {
            Blockchain blockchain = new Blockchain();
            
            blockchain.AddBlock(new Block(Convert.ToDateTime(DateTime.Now), null, block));
            
            Block b = null;
            
            if (isCompleted)
            {
            }

            if (blockchain.IsValid())
            {
                foreach (var item in blockchain.Node)
                {
                    b = item;
                }

                var artefact = JsonConvert.SerializeObject(b, formatting: Formatting.Indented);
                return artefact.ToString();
            }

            return null;
        }

        public static string GetTimeStamp(DateTime dateTime)
        {
            return dateTime.ToString("yyyy_MM_dd_THHmss_ffff"); ;
        }

        public static string Base64Encode(string toBase64)
        {
            var toBase64Bytes = Encoding.UTF8.GetBytes(toBase64);
            return Convert.ToBase64String(toBase64Bytes);
        }

        public static string Base64Decode(string base64Data)
        {
            var base64DataBytes = Convert.FromBase64String(base64Data);
            return Encoding.UTF8.GetString(base64DataBytes);
        }

        public static Guid GetGuid(string toGuid)
        {
            byte[] toGuidBytes = Encoding.UTF8.GetBytes(toGuid);
            byte[] hashedBytes = new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(toGuidBytes);
            Array.Resize(ref hashedBytes, 16);

            return new Guid(hashedBytes);
        }

        public static string Encrypt(string uuid)
        {
            try
            {
                string toReturn = "";
                string publicKey = "12345678";
                string secretKey = "87654321";
                byte[] secretKeyByte = { };
                byte[] publicKeyByte = { };
                secretKeyByte = Encoding.UTF8.GetBytes(secretKey);
                publicKeyByte = Encoding.UTF8.GetBytes(publicKey);

                MemoryStream ms = null;
                CryptoStream cs = null;

                byte[] inputByteArray = Encoding.UTF8.GetBytes(uuid);

                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publicKeyByte, secretKeyByte), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    toReturn = Convert.ToBase64String(ms.ToArray());
                }

                return toReturn;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public static string Decrypt(string encrypt)
        {
            try
            {

                string toReturn = "";
                string publicKey = "12345678";
                string secretKey = "87654321";
                byte[] privateKeyByte = { };
                byte[] publicKeyByte = { };
                privateKeyByte = Encoding.UTF8.GetBytes(secretKey);
                publicKeyByte = Encoding.UTF8.GetBytes(publicKey);

                MemoryStream ms = null;
                CryptoStream cs = null;

                byte[] inputByteArray = new byte[encrypt.Replace(" ", "+").Length];
                inputByteArray = Convert.FromBase64String(encrypt.Replace(" ", "+"));

                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publicKeyByte, privateKeyByte), CryptoStreamMode.Write);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    toReturn = encoding.GetString(ms.ToArray());
                }

                return toReturn;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }
    }
}
