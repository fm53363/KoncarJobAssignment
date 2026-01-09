using Shared.Protocol;
using System.Net.Sockets;

namespace Client
{
    internal class ConsoleUI
    {

        public static void Run()
        {
            const string serverIp = "127.0.0.1";
            const int port = 5000;
            try
            {
                using var client = new MyTcpClient(serverIp, port);
                client.Connect();

                Console.WriteLine("Connected to server.");

                while (true)
                {
                    ShowMenu();
                    string? choice = Console.ReadLine();

                    if (choice == "6" || choice == null)
                    {
                        break;
                    }

                    var req = HandleChoice(choice);
                    client.Send(req);
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

        private static Request HandleChoice(string choice)
        {

            var req = new Request();
            switch (choice)
            {
                case "1":
                    req = BuildGetAllRequest();
                    break;
                case "2":
                    req = BuildGetByIdRequest();
                    break;
                case "3":
                    req = BuildCreateRequest();
                    break;
                case "4":
                    req = BuildUpdateRequest();
                    break;
                case "5":
                    req = BuildDeleteRequest();
                    break;
                default:
                    req = BuildGetAllRequest();
                    break;
            }
            return req;
        }


        private static int ReadId()
        {
            while (true)
            {
                Console.Write(" Id:");
                if (int.TryParse(Console.ReadLine(), out int value))
                    return value;
                Console.WriteLine(" Invalid number, try again.");
            }
        }

        private static string ReadString(string prompt)
        {
            Console.Write(" " + prompt + ": ");
            return Console.ReadLine() ?? string.Empty;
        }



        private static Request BuildGetAllRequest()
        {
            return Request.GetAll();

        }

        private static Request BuildGetByIdRequest()
        {
            int id = ReadId();
            return Request.GetById(id);

        }

        private static Request BuildCreateRequest()
        {
            String title = ReadString("Title");
            String desc = ReadString("Description");
            return Request.Create(title, desc);

        }

        private static Request BuildUpdateRequest()
        {
            int id = ReadId();
            String title = ReadString("Title");
            String desc = ReadString("Description");
            return Request.Update(id, title, desc);
        }

        private static Request BuildDeleteRequest()
        {
            int id = ReadId();
            return Request.Delete(id);
        }
        private static void ShowMenu()
        {
            Console.WriteLine("\n====== MENI ======");

            Console.WriteLine("1. GetAll");
            Console.WriteLine("2. GetById");
            Console.WriteLine("3. Create");
            Console.WriteLine("4. Update");
            Console.WriteLine("5. Delete");

            Console.WriteLine("6. Izlaz");

            Console.Write("Odaberi naredbu: ");
            return;
        }
    }


}
