using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParcelBlock;

namespace ParcelService_license
{
    public class TestCase
    {
        private int _userType = 0x00;
        private string _userJson = string.Empty;
        private string _userName = string.Empty;
        private string _password = string.Empty;
        private bool _isEmployee = false;
        private bool _isCompleted = false;

        public bool IsCompleted { get => _isCompleted; set => _isCompleted = value; }

        internal static void DoLicense()
        {
            bool showMenu = true;

            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }

        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Chose an option:");
            Console.WriteLine("1) Generate license");
            Console.WriteLine("2) Exit");
            Console.WriteLine("\r\nSelect an option: ");

            TestCase test = new TestCase();
            ParcelMain parcelMain = new ParcelMain();
            ParcelEncryption parcelEncryption = new ParcelEncryption();

            try
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        var usrName = GetName(test);
                        var usrType = GetType(test);
                        var usrPass = GetPassword(test);

                        string sUsrType = test._isEmployee ? "employee" : "customer";

                        test._userJson = "{user:" + sUsrType + ",name:" + usrName + "}";

                        parcelMain.Input = test._userJson;
                        parcelMain.EncryptInput = usrPass;

                        var newPassword = parcelMain.CreateNewPassword(parcelMain);

                        parcelMain.Password = parcelMain.EncryptOutput;
                        parcelMain.User_Auth_Json = "{code:" + parcelMain.Uuid + ",user:" + sUsrType + ",name:" + usrName + "}";
                        
                        parcelMain = parcelMain.GetAuthorize(parcelMain, test.IsCompleted);

                        string parcelJson = JsonConvert.SerializeObject(parcelMain, Formatting.Indented);

                        DisplayResult(parcelJson);

                        return true;
                    case "2":
                        return false;
                    default:
                        return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        private static string GetName(TestCase test)
        {
            bool bit = false;

            do
            {
                Console.WriteLine("-- Insert User Name and press Enter");
                test._userName = Console.ReadLine();
                bit = string.IsNullOrEmpty(test._userName);
            } while (bit);

            return test._userName;
        }

        private static int GetType(TestCase test)
        {
            do
            {
                Console.WriteLine("-- Insert User Type: 1 or 2 and press Enter");
                Console.WriteLine("-- -- 1) Employee");
                Console.WriteLine("-- -- 2) Customer");
                test._userType = Convert.ToInt32(Console.ReadLine());
            } while (test._userType != 1 && test._userType != 2);
            if (test._userType == 1) test._isEmployee = true;
            return test._userType;
        }

        private static string GetPassword(TestCase test)
        {
            bool bit = false;
            do
            {
                Console.WriteLine("-- Insert Password and press Enter");
                test._password = Console.ReadLine();
                bit = string.IsNullOrEmpty(test._password);
            } while (bit);

            return test._password;
        }

        private static void DisplayResult(string message)
        {
            Console.WriteLine($"\r\nYour result is: {message}");
            Console.WriteLine("\r\nPress Enter to return to Main Menu");
            Console.ReadLine();
        }
    }
}
