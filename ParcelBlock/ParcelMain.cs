using Newtonsoft.Json;
using System;
using System.Linq;

namespace ParcelBlock
{
    public class ParcelMain
    {
        private string _input;
        //private string _output;
        private string _encryptInput;
        private string _encryptOutput;
        private string _base64;
        private string _uuid;
        private string _encrypted;
        private string _block;
        private string _password;
        private string _license;
        private string _user_json;

        public string Input { get => _input; set => _input = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        //public string Output { get => _output; set => _output = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        public string EncryptInput { get => _encryptInput; set => _encryptInput = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        public string EncryptOutput { get => _encryptOutput; set => _encryptOutput = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        public string Base64 { get => _base64; set => _base64 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        public string Uuid { get => _uuid; set => _uuid = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        public string Encrypted { get => _encrypted; set => _encrypted = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        public string License { get => _license; set => _license = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        public string Password { get => _password; set => _password = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        public string Block { get => _block; set => _block = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        public string User_Auth_Json { get => _user_json; set => _user_json = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }


        public virtual bool ValidateAccess(string password)
        {
            if (ParcelEncryption.Decrypt(Password) != password) return false;
            return true;
        }

        public virtual bool IsValid(string password)
        {
            if (password.Length <= 0 && password.Length < 7) return false;
            if (string.IsNullOrEmpty(password)) return false;
            if (string.IsNullOrWhiteSpace(password)) return false;
            return true;
        }

        public ParcelMain GetAuthorize(ParcelMain parcel, bool isCompleted)
        {
            string userToRemove = "user";
            string nameToRemove = "name";
            string[] input = parcel.Input.Split(',', '{', '}', ':');

            input = input.Where(item => item != userToRemove && item != nameToRemove && item != "").ToArray();

            parcel.Base64 = ParcelTools.Base64Encode(parcel.Input);
            parcel.Uuid = ParcelTools.GetGuid(parcel.Base64).ToString();
            parcel.Encrypted = ParcelTools.Encrypt(parcel.Uuid);
            parcel.Block = ParcelTools.GetBlockchain("{code: " + parcel.Uuid + ",user: " + input[0] + ",name: " + input[1], isCompleted);

            Chain chain = JsonConvert.DeserializeObject<Chain>(parcel.Block);

            parcel.License = chain.Hash;
            parcel.Block = JsonConvert.SerializeObject(chain, formatting: Formatting.Indented);
            parcel.User_Auth_Json = "{code:" + parcel.Uuid + ",auth:" + chain.Hash + ",user:" + input[0] + ",name:" + input[1] + "}";

            return parcel;
        }

        public ParcelMain CreateNewPassword(ParcelMain parcel)
        {
            parcel.EncryptOutput = ParcelEncryption.Encrypt(parcel.EncryptInput);
            return parcel;
        }
    }
}
