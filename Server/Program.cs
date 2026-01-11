using Server.Networking;
using Server.Repositories;
using Server.Services;
using Server.Sql;

class Program
{
    public static async Task Main()
    {

        ICharacterRepository repo = new InMemoryCharacterRepository();
        ISqlCommandWriter sqlWriter = new FileSqlCommandWriter();
        ICharacterService service = new CharacterService(repo, sqlWriter);

        RequestHandler requestHandler = new RequestHandler(service);

        MyTcpServer server = new MyTcpServer(requestHandler);
        await server.StartAsync();
    }

}