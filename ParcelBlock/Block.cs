using System;
using System.Security.Cryptography;
using System.Text;

namespace ParcelBlock
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public string Data { get; set; }
        public int Nonce { get; set; } = 0;

        public Block(DateTime dateTime, string previousHash, string data)
        {
            Index = 0;
            TimeStamp = dateTime;
            PreviousHash = previousHash;
            Data = data;
            Hash = CalculateHash();
        }

        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{Data}-{Nonce}");
            byte[] outpuBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outpuBytes);
        }

        public void Mine(int difficulty)
        {
            var leadingZeros = new string('0', difficulty);

            while (this.Hash == null || this.Hash.Substring(0, difficulty) != leadingZeros)
            {
                this.Nonce++;
                this.Hash = this.CalculateHash();
            }
        }
    }
}
