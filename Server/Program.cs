using Server.Networking;

class Program
{
    public static async Task Main()
    {
        MyTcpServer server = new MyTcpServer();
        await server.StartAsync();
    }

}