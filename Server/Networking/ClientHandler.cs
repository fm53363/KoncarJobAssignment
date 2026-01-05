using System.Net.Sockets;

namespace Server.Networking
{
    internal class ClientHandler
    {

        private readonly TcpClient _client;


        public int ClientId { get; }

        private static int _nextId = 0;

        public ClientHandler(TcpClient client)
        {
            _client = client;
            ClientId = Interlocked.Increment(ref _nextId);
        }



        private void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] " +
                $"[Client {ClientId}] " +
                $"[Thread {Thread.CurrentThread.ManagedThreadId}] " +
                $"{message}");
        }


        public async Task HandleAsync()
        {
            using var stream = _client.GetStream();
            byte[] buffer = new byte[1024];
            try
            {
                while (true)
                {
                    Log("reading from stream");
                    int bytes = await stream.ReadAsync(buffer);
                    if (bytes == 0)
                        break;
                    string msg = System.Text.Encoding.UTF8.GetString(buffer, 0, bytes);
                    Log($"Received: {msg}");

                    string response = "Echo: " + msg;
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(response);
                    await stream.WriteAsync(data);
                    Log("after writing to stream");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client error: " + ex.Message);
            }
            finally { _client.Close(); Console.WriteLine("Client disconnected."); }
        }
    }
}
