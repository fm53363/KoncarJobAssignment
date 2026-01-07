using System.Net.Sockets;
using System.Text;

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
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            try
            {
                while (true)
                {
                    Log("reading from stream");
                    string? msg = await reader.ReadLineAsync();
                    if (msg is null) // client closed connection
                        break;

                    Log($"Received: {msg}");

                    // TODO: parse JSON and execute CRUD; for now echo
                    string response = "Echo: " + msg;
                    await writer.WriteLineAsync(response);
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
