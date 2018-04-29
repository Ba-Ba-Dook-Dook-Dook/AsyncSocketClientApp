using System;
using AsyncSocketLib;

namespace AsyncSocketClientApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AsyncSocketClient socketClient = new AsyncSocketClient();
            socketClient.RaiseMessageReceivedEvent += HandleMessageReceived;
            socketClient.RaiseServerConnectedEvent += HandleServerConnected;
            socketClient.RaiseServerDisconnectedEvent += HandleServerDisconnected;

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

                if (strInput != null && strInput.Trim() != "<EXIT>")
                {
                    socketClient.SendToServer(strInput.Trim());

                }else if (strInput != null && strInput.Trim() == "<EXIT>")
                {
                    socketClient.CloseAndDisconnect();
                }

            } while (strInput != "<EXIT>");


        }

        private static void HandleServerDisconnected(object sender, ServerDisconnectedEventArgs e)
        {
            Console.WriteLine($"{DateTime.Now} - Disconected From: {e.Server}:{e.Port}{Environment.NewLine}");
        }

        private static void HandleServerConnected(object sender, ServerConnectedEventArgs e)
        {
            Console.WriteLine($"{DateTime.Now} - Connected to: {e.Server}:{e.Port}{Environment.NewLine}");
        }

        private static void HandleMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine($"{DateTime.Now} - Received: {e.MessageReceived}{Environment.NewLine}");
        }
    }
}
