using System.Net.Sockets;
using System.Text;


namespace Client
{
    public class MyTcpClient : IDisposable
    {
        private TcpClient? _client;
        private NetworkStream? _stream;
        public bool IsConnected => _client?.Connected ?? false;

        public MyTcpClient(string server = "127.0.0.1", int port = 50000)
        {

            _client = new TcpClient(server, port);
            _stream = _client.GetStream();


        }

        public string? SendMessage(string message)
        {

            if (!IsConnected || _stream == null)
                throw new InvalidOperationException("Not connected to server.");

            byte[] data = Encoding.ASCII.GetBytes(message);
            _stream.Write(data, 0, data.Length);

            byte[] buffer = new byte[256];
            int bytesRead = _stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            return response;



        }

        public void Dispose()
        {
            _stream?.Close();
            _client?.Close();
        }
    }

}


