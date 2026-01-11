using Shared.Protocol;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Server.Networking
{
    internal class ClientHandler
    {

        private readonly TcpClient _client;
        private readonly RequestHandler _handler;


        public int ClientId { get; }

        private static int _nextId = 0;

        public ClientHandler(TcpClient client, RequestHandler handler)
        {
            _client = client;
            _handler = handler;
            ClientId = Interlocked.Increment(ref _nextId);
        }



        private void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] " +
                $"[Hander for Client {ClientId}] " +
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
                    Log("waiting for requests");
                    string? msg = await reader.ReadLineAsync();
                    if (msg is null) // client closed connection
                        break;

                    Log($"Received: {msg}");

                    Request request = Request.FromJson(msg);

                    Response response = await _handler.Handle(request);

                    await writer.WriteLineAsync(response.ToJson());
                    Log("after writing to stream");
                }
            }
            catch (JsonException ex)
            {
                Log("Invalid JSON received");

                var errorResponse = Response.Error("Invalid request format");
                await writer.WriteLineAsync(errorResponse.ToJson());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client error: " + ex.Message);
            }
            finally { _client.Close(); Console.WriteLine("Client disconnected."); }
        }
    }
}
