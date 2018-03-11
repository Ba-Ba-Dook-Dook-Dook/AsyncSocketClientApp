using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace AsyncSocketLib
{
    public class AsyncSocketServer
    {
        private IPAddress _ipAddress;
        private int _port;
        private TcpListener _tcpListener;

        public bool KeepRunning { get; set; }


        public async void StartListeningForIncomingConnection(IPAddress ipAddress = null, int port = 23000)
        {
            if (ipAddress == null)
                ipAddress = IPAddress.Any;

            if (port <= 0)
                port = 23000;

            _ipAddress = ipAddress;
            _port = port;

            Debug.WriteLine($"IP Address: {_ipAddress} - Port: {_port}.");

            _tcpListener = new TcpListener(_ipAddress, _port);

            try
            {
                _tcpListener.Start();

                KeepRunning = true;

                while (KeepRunning)
                {
                    var returnedByAccept = await _tcpListener.AcceptTcpClientAsync();

                    Debug.WriteLine($"Client connected successfully: {returnedByAccept}.");

                    TakeCaraOfTCPClient(returnedByAccept);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }




        }

        private async void TakeCaraOfTCPClient(TcpClient tcpClient)
        {
            NetworkStream networkStream = null;
            StreamReader streamReader = null;

            try
            {
                networkStream = tcpClient.GetStream();
                streamReader = new StreamReader(networkStream);

                var buff = new char[64];

                while (KeepRunning)
                {
                    Debug.WriteLine("*** Ready to read");

                    var nRet = await streamReader.ReadAsync(buff, 0, buff.Length);

                    Debug.WriteLine($"Returned: {nRet}");

                    if (nRet == 0)
                    {
                        Debug.WriteLine("Socket disconnected.");
                        break;
                    }

                    var receivedText = new string(buff);

                    Debug.WriteLine($"*** RECEIVED: {receivedText}");

                    Array.Clear(buff,0, buff.Length);
                   
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

            }
        }
    }
}
