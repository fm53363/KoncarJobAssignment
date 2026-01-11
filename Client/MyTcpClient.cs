using Shared.Protocol;
using System.Net.Sockets;
using System.Text;

public class MyTcpClient : IDisposable
{
    private TcpClient? _tcpClient;
    private StreamReader? _reader;
    private StreamWriter? _writer;
    private readonly string _serverIp;
    private readonly int _port;

    public bool IsConnected => _tcpClient?.Connected ?? false;

    public MyTcpClient(string serverIp, int port)
    {
        _serverIp = serverIp;
        _port = port;
    }

    public void Connect()
    {
        if (IsConnected)
            return;

        _tcpClient = new TcpClient(_serverIp, _port);
        var stream = _tcpClient.GetStream();

        _reader = new StreamReader(stream, Encoding.UTF8);
        _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
    }


    public string? Send(Request req)
    {
        if (!IsConnected || _writer == null || _reader == null)
            throw new InvalidOperationException("Not connected to server.");

        try
        {
            // 1. Serijaliziraj Request u JSON
            string requestJson = req.ToJson();

            // 2. Pošalji 
            _writer.WriteLine(requestJson);
            Console.WriteLine($"[SENDING] {requestJson}");



            // 3. Primi odgovor (ReadLine čita do \n)
            string? responseJson = _reader.ReadLine();
            if (responseJson == null)
                throw new IOException("Connection closed by server");

            Console.WriteLine($"[RECEIVED] {responseJson}");

            return responseJson;
        }
        catch (IOException ex)
        {
            Console.WriteLine($"[ERROR] Connection error: {ex.Message}");
            Dispose();
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
            throw;
        }
    }


    public void Dispose()
    {
        _reader?.Dispose();
        _writer?.Dispose();

        _tcpClient?.Close();
        _reader = null;
        _writer = null;
        _tcpClient = null;
    }
}