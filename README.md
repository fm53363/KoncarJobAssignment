# KoncarJobAssignment


## Project Structure and Class Responsibilities
- `Client/`
  - `MyTcpClient` — TCP client that sends newline-delimited JSON `Request` objects and reads newline-delimited JSON responses.
  - `ConsoleUI` — interactive console menu that builds `Request` objects and sends them through `MyTcpClient`.
  - `Program` — app entry point that runs the interactive UI.

- `Server/Networking`
  - `MyTcpServer` — TCP listener and accept loop. Creates a `ClientHandler` per connection.
  - `ClientHandler` — per-connection handler: reads requests (line-delimited JSON), forwards them to `RequestHandler`, and writes responses.
  - `RequestHandler` — parses `Request`, validates fields, maps actions to `ICharacterService` calls, and produces `Response` objects.
  
- `Server/Repositories/`
  - `ICharacterRepository` — repository interface for CRUD operations on `Character` objects.
  - `InMemoryCharacterRepository` — in-memory implementation used for this assignment.
  
- `Server/Services/`
  - `ICharacterService` — service interface encapsulating business logic and persistence calls.
  - `CharacterService` — service implementation coordinating CRUD operations via `ICharacterRepository` and `ISqlCommandWriter`; thread-safe, using [SemaphoreSlim](https://learn.microsoft.com/en-us/dotnet/api/system.threading.semaphoreslim) to serialize access because the repository and SQL writer implementations are not thread-safe.
  
- `Server/Sql/`
  - `ISqlCommandWriter` — abstraction for writing SQL commands.
  - `FileSqlCommandWriter` — appends SQL commands to a text file (`myDB.txt` by default).
  
- `Shared/Protocol/`
  - `Request` — DTO for client requests (action, id, title, desc). Serialized with null properties omitted.
  - `Response` — DTO for server responses (`status`, optional `message`, optional `result`).

- `Shared/Model/`
  - `Character` — object with Id, Title, Description


## Example Request / Response (newline-delimited JSON)
Requests and responses are UTF-8 JSON, one object per line. Project supports operations: GetAll, GetById, Update, Create, Delete.

- Create request:

    ```json
    {"Operation":"Create","Title":"ime","Desc":"ovo je opis"}
    ```

- GetById id request:

    ```json
    {"Operation":"GetById","Id":3}
    ```

- Successful response example:

    ```json
    {"Status":"ok","Message":"Character created","Item":{"Id":0,"Title":"ime","Desc":"ovo je opis"}}   
    ```

- Error response example:

    ```json
    {"Status":"error","Message":"Id not present"}
    ```

**Notes:**
- Serialization omits null properties so messages are compact.
- Server and client both use `ReadLine`/`WriteLine` semantics (newline framing).

## Quick Start
### Prerequisites:
- .NET 8 SDK

From the repository root:

1. Start the server:

    ```bash
    dotnet run --project Server
    ```

   The server listens on port `5000` by default and writes simulated SQL commands to `myDB.txt` in the server working directory.

2. Start a single interactive client (menu):

    ```bash
    dotnet run --project Client
    ```

## Tests
- Test project: `Server.Tests` (xUnit).
- Frameworks/libraries used: `xUnit` for test runner and `Moq` for mocking dependencies.
- What is tested: Unit tests primarily cover `CharacterService` behavior (create, read, update, delete) and verify interactions with `ICharacterRepository` and `ISqlCommandWriter`.



- Notes:
  - Tests mock `ICharacterRepository` and `ISqlCommandWriter` to isolate `CharacterService` logic and to assert SQL command text passed to the writer.
  - If any interfaces or types used in tests are `internal`, add the following attribute to the assembly that defines them so Moq (DynamicProxy) can create proxies:

    ```csharp
    [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("DynamicProxyGenAssembly2")]
    ```

    Alternatively, make the interfaces `public`.
  - Tests use synchronous setups that return `Task.CompletedTask` for asynchronous writer calls when appropriate.



