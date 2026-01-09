using Shared.Protocol;
using System.Net.Sockets;
using System.Text;

public class MyTcpClient : IDisposable
{
    private TcpClient? _tcpClient;
    private NetworkStream? _stream;
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
        _stream = _tcpClient.GetStream();
    }


    public string? Send(Request req)
    {
        if (!IsConnected || _stream == null)
            throw new InvalidOperationException("Not connected to server.");

        try
        {
            // 1. Serijaliziraj Request u JSON
            string requestJson = req.ToJson();

            // 2. Dodaj newline kao delimiter (da server zna gdje poruka završava)
            string message = requestJson + "\n";

            // 3. Konvertiraj u byte array
            byte[] data = Encoding.UTF8.GetBytes(message);

            // 4. Pošalji na server
            _stream.Write(data, 0, data.Length);
            Console.WriteLine($"[ŠALJEM:] {requestJson}");

            // 5. Čitaj odgovor do newline-a
            string response = ReadLine(_stream);
            Console.WriteLine($"[PRIMAM] {response}");

            return response;
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


    private string ReadLine(NetworkStream stream)
    {
        var buffer = new List<byte>();
        int b;

        while ((b = stream.ReadByte()) != -1)
        {
            if (b == '\n')  // Newline je delimiter
                break;

            buffer.Add((byte)b);
        }

        if (buffer.Count == 0)
            throw new IOException("Connection closed by server");

        return Encoding.UTF8.GetString(buffer.ToArray());
    }


    /*
    // ============================================================================
    // CRUD Wrapper metode (za lakšu upotrebu)
    // ============================================================================

    /// <summary>
    /// Dohvaća sve likove (List)
    /// </summary>
    public Response GetAll()
    {
        if (!IsConnected)
            Connect();

        var request = Request.List();
        string? responseJson = Send(request);
        if (responseJson != null)
            return Response.FromJson(responseJson);
    }

    /// <summary>
    /// Dohvaća lika po ID-u (Read)
    /// </summary>
    public Response GetById(int id)
    {
        if (!IsConnected)
            Connect();

        var request = Request.Read(id);
        string? responseJson = Send(request);
        return Response.FromJson(responseJson);
    }

    /// <summary>
    /// Kreira novog lika (Create)
    /// </summary>
    public Response Create(string title, string desc)
    {
        if (!IsConnected)
            Connect();

        var request = Request.Create(title, desc);
        string? responseJson = Send(request);
        return Response.FromJson(responseJson);
    }

    /// <summary>
    /// Ažurira postojećeg lika (Update)
    /// </summary>
    public Response Update(int id, string title, string desc)
    {
        if (!IsConnected)
            Connect();

        var request = Request.Update(id, title, desc);
        string? responseJson = Send(request);
        return Response.FromJson(responseJson);
    }

    /// <summary>
    /// Briše lika (Delete)
    /// </summary>
    public Response Delete(int id)
    {
        if (!IsConnected)
            Connect();

        var request = Request.Delete(id);
        string? responseJson = Send(request);
        return Response.FromJson(responseJson);
    }
    */


    // IDisposable pattern
    public void Dispose()
    {
        _stream?.Close();
        _tcpClient?.Close();
        _stream = null;
        _tcpClient = null;
    }
}