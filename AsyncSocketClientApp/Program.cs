using System;
using AsyncSocketLib;

namespace AsyncSocketClientApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AsyncSocketClient socketClient = new AsyncSocketClient();

            Console.WriteLine("Async Socket Client: Started!");

            Console.WriteLine("Please type a valid server IP Address and press enter:");
            var strServerIpAddress = Console.ReadLine();



            Console.WriteLine("Please type a valid port and press enter:");
            var strPort = Console.ReadLine();


            if (!socketClient.SetServerIpAddress(strServerIpAddress) || !socketClient.SetServerPort(strPort))
            {
                Console.WriteLine("Wrong IP or Port supplied - {0} - {1} - Press a key to exit!",
                    strServerIpAddress,
                    strPort);

                Console.ReadKey();

                return;
            }

            socketClient.ConnectToServer();

            string strInput = null;

            do
            {
                strInput = Console.ReadLine();

            } while (strInput != "<EXIT>");


        }
    }
}
