using System.Net;
using System.Net.Sockets;

namespace Server.Networking
{
    public class MyTcpServer
    {

        private readonly TcpListener _listener;
        private bool _running;


        public MyTcpServer(string ip = "127.0.0.1", int port = 5000)
        {
            _listener = new TcpListener(IPAddress.Parse(ip), port);
        }

        public async Task StartAsync()
        {
            _listener.Start();
            _running = true;
            Console.WriteLine("Async TCP server started...");
            while (_running)
            {
                var client = await _listener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected!");

                var handler = new ClientHandler(client);
                _ = handler.HandleAsync();
            }
        }

        public void Stop()
        {
            _running = false;
            _listener.Stop();
        }





    }
}
