using Client;
using System.Net.Sockets;

class Program
{
    public static void Main()
    {
        const string serverIp = "127.0.0.1";
        const int port = 5000;
        try
        {
            // Create TCP client; using var ensures automatic disposal
            using var client = new MyTcpClient(serverIp, port);

            Console.WriteLine("Connected to server. Enter commands (type EXIT to quit):");

            string? input;
            while ((input = Console.ReadLine()) != null)
            {
                if (input.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                    break;

                // Send command to server and receive response
                string? response = client.SendMessage(input);

                if (response != null)
                    Console.WriteLine("Server: " + response);
            }

            Console.WriteLine("Client disconnected.");
        }
        catch (SocketException ex)
        {
            Console.WriteLine("Could not connect to server: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}

