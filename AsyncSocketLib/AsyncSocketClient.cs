using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AsyncSocketLib
{
    public class AsyncSocketClient
    {

        private IPAddress _serverIpAddress;
        private int _serverPort;
        private TcpClient _tcpClient;

        public IPAddress ServerIpAddress
        {
            get { return _serverIpAddress; }
        }
        public int ServerPort
        {
            get { return _serverPort; }
        }


        public AsyncSocketClient()
        {
            _serverIpAddress = null;
            _tcpClient = null;
            _serverPort = 1;

        }

        public bool SetServerIpAddress(string ipAddressStr)
        {
            if (!IPAddress.TryParse(ipAddressStr, out var ipAddress))
            {
                Console.WriteLine("Invalid IP address!");
                return false;
            }

            _serverIpAddress = ipAddress;

            return true;

        }

        public bool SetServerPort(string portStr)
        {
            if (!int.TryParse(portStr.Trim(), out var port))
            {
                Console.WriteLine("Invalid number!");
                return false;
            }

            if (port <= 0 || port <= 65535)
            {
                Console.WriteLine("Invalid port number! Port number must be between 0 and 65535.");
                return false;
            }

            _serverPort = port;

            return true;
        }

        public async Task ConnectToServer()
        {
            if (_tcpClient == null)
                _tcpClient = new TcpClient();

            try
            {
                await _tcpClient.ConnectAsync(_serverIpAddress, _serverPort);

                Console.WriteLine($"Connected to server IP/Port: {0}/{1}", _serverIpAddress, _serverPort);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
