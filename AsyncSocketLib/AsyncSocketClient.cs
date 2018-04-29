using System;
using System.IO;
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

        public EventHandler<ServerDisconnectedEventArgs> RaiseServerDisconnectedEvent;
        public EventHandler<ServerConnectedEventArgs> RaiseServerConnectedEvent;
        public EventHandler<MessageReceivedEventArgs> RaiseMessageReceivedEvent;

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

            if (port <= 0 || port > 65535)
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

                OnRaiseServerConnectedEvent(new ServerConnectedEventArgs(_serverIpAddress.ToString(), ServerPort.ToString()));

                await ReadDataAsync(_tcpClient);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task ReadDataAsync(TcpClient tcpClient)
        {
            try
            {
                var clientStreamReader = new StreamReader(tcpClient.GetStream());
                var buff = new char[64];

                while (true)
                {
                    var readByteCount = await clientStreamReader.ReadAsync(buff, 0, buff.Length);

                    if (readByteCount <= 0)
                    {
                        OnRaiseServerDisconnectedEvent(new ServerDisconnectedEventArgs(tcpClient.Client.LocalEndPoint.ToString(), tcpClient.Client.RemoteEndPoint.ToString()));
                        tcpClient.Close();
                        break;
                    }


                    OnRaiseMessageReceivedEvent(new MessageReceivedEventArgs(tcpClient.Client.RemoteEndPoint.ToString(),
                        new string(buff)));

                    Array.Clear(buff, 0, buff.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }



        public async Task SendToServer(string message)
        {

            if (string.IsNullOrEmpty(message))
            {
                Console.WriteLine("Empty string supplied to send!");
                return;
            }

            if (_tcpClient == null || !_tcpClient.Connected) return;

            var clientStreamWriter = new StreamWriter(_tcpClient.GetStream()) { AutoFlush = true };

            await clientStreamWriter.WriteAsync(message.Trim());

            Console.WriteLine("Message sended!");
        }

        public async Task CloseAndDisconnect()
        {

            if (_tcpClient != null && _tcpClient.Connected)
                _tcpClient.Close();

        }

        protected virtual void OnRaiseMessageReceivedEvent(MessageReceivedEventArgs e)
        {
            var handler = RaiseMessageReceivedEvent;

            handler?.Invoke(this, e);
        }

        private void OnRaiseServerDisconnectedEvent(ServerDisconnectedEventArgs serverDisconnectedEventArgs)
        {
            var handler = RaiseServerDisconnectedEvent;

            handler?.Invoke(this, serverDisconnectedEventArgs);
        }

        private void OnRaiseServerConnectedEvent(ServerConnectedEventArgs serverConnectedEventArgs)
        {
            var handler = RaiseServerConnectedEvent;

            handler?.Invoke(this, serverConnectedEventArgs);
        }
    }
}
