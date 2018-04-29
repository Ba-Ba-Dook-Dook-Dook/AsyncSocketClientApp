using System;

namespace AsyncSocketLib
{
    public class ClientConnetedEventArgs : EventArgs
    {
        public string NewClient { get; set; }

        public ClientConnetedEventArgs(string newClient)
        {
            NewClient = newClient;
        }

    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public string Client { get; set; }
        public string MessageReceived { get; set; }

        public MessageReceivedEventArgs(string client, string messageReceived)
        {
            Client = client;
            MessageReceived = messageReceived;
        }
    }

    public class ServerDisconnectedEventArgs : EventArgs
    {
        public string Port { get; set; }
        public string Server { get; set; }

        public ServerDisconnectedEventArgs(string server, string port)
        {
            Server = server;
            Port = port;
        }
    }

    public class ServerConnectedEventArgs : EventArgs
    {
        public string Port { get; set; }
        public string Server { get; set; }

        public ServerConnectedEventArgs(string server, string port)
        {
            Server = server;
            Port = port;
        }
    }
    

}