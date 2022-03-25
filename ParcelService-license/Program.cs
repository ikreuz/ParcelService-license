using System;
using System.Diagnostics;

namespace ParcelService_license
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                TestCase.DoLicense();
                sw.Stop();
                Console.WriteLine("Time:{0}ms", sw.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Finished. total time={0}ms", sw.ElapsedMilliseconds);
                Console.WriteLine("Finished. Press enter key to continue.");
                Console.ReadLine();
            }
        }
    }
}
