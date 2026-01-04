using System.Net.Sockets;

namespace Server.Networking
{
    internal class ClientHandler
    {

        private readonly TcpClient _client;

        public ClientHandler(TcpClient client)
        {
            _client = client;
        }


        public async Task HandleAsync()
        {
            using var stream = _client.GetStream();
            byte[] buffer = new byte[1024];
            try
            {
                while (true)
                {
                    int bytes = await stream.ReadAsync(buffer);
                    if (bytes == 0)
                        break;
                    string msg = System.Text.Encoding.UTF8.GetString(buffer, 0, bytes);
                    Console.WriteLine($"Received: {msg}");
                    string response = "Echo: " + msg;
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(response);
                    await stream.WriteAsync(data);
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
