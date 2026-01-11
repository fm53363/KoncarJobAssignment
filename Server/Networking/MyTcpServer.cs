using System.Net;
using System.Net.Sockets;

namespace Server.Networking
{
    internal class MyTcpServer
    {

        private readonly TcpListener _listener;
        private readonly RequestHandler _handler;
        private bool _running;


        public MyTcpServer(RequestHandler handler, int port = 5000)
        {
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            _handler = handler;
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

                var handler = new ClientHandler(client, _handler);
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
