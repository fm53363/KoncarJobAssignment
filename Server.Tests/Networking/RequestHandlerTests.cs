using Moq;
using Server.Networking;
using Server.Services;
using Shared.Models;
using Shared.Protocol;

namespace Server.Tests.Networking
{
    public class RequestHandlerTests
    {
        private readonly Mock<ICharacterService> _serviceMock;
        private readonly RequestHandler _handler;

        public RequestHandlerTests()
        {
            _serviceMock = new Mock<ICharacterService>();
            _handler = new RequestHandler(_serviceMock.Object);
        }

        // --- GETALL ---
        [Fact]
        public async Task Handle_WhenGetAll_ReturnsOkResponseWithItems()
        {
            var characters = new List<Character> { new Character { Id = 1, Title = "Hero" } };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(characters);

            var request = new Request { Operation = RequestType.GetAll };

            var response = await _handler.Handle(request);

            Assert.Equal("Characters retrieved", response.Message);
            Assert.Equal("ok", response.Status);

            Assert.NotNull(response.Items);
            Assert.Single(response.Items!);
            Assert.Null(response.Item);
        }

        // --- GETBYID ---
        [Fact]
        public async Task Handle_WhenGetByIdAndCharacterExists_ReturnsOkResponseWithItem()
        {
            var character = new Character { Id = 1, Title = "Hero" };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(character);

            var request = new Request { Operation = RequestType.GetById, Id = 1 };

            var response = await _handler.Handle(request);

            Assert.Equal("Character found", response.Message);
            Assert.Equal("ok", response.Status);
            Assert.NotNull(response.Item);
            Assert.Equal(character.Id, response.Item!.Id);
            Assert.Null(response.Items);
        }

        [Fact]
        public async Task Handle_WhenGetByIdAndCharacterDoesNotExist_ReturnsErrorResponse()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Character?)null);

            var request = new Request { Operation = RequestType.GetById, Id = 99 };

            var response = await _handler.Handle(request);

            Assert.Equal("error", response.Status);
            Assert.Equal("Id not present", response.Message);
            Assert.Null(response.Item);
            Assert.Null(response.Items);
        }

        [Fact]
        public async Task Handle_WhenGetByIdAndIdIsNull_ReturnsErrorResponse()
        {
            var request = new Request { Operation = RequestType.GetById, Id = null };

            var response = await _handler.Handle(request);

            Assert.Equal("error", response.Status);
            Assert.Equal("Id not present", response.Message);
            Assert.Null(response.Item);
            Assert.Null(response.Items);
        }

        // --- CREATE ---
        [Fact]
        public async Task Handle_WhenCreate_ReturnsOkResponseWithItem()
        {
            var character = new Character { Title = "Hero", Desc = "Brave" };
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<Character>())).ReturnsAsync(character);

            var request = new Request { Operation = RequestType.Create, Title = "Hero", Desc = "Brave" };

            var response = await _handler.Handle(request);

            Assert.Equal("ok", response.Status);
            Assert.Equal("Character created", response.Message);
            Assert.NotNull(response.Item);
            Assert.Equal(character.Title, response.Item!.Title);
            Assert.Null(response.Items);
        }

        // --- UPDATE ---
        [Fact]
        public async Task Handle_WhenUpdateAndIdExists_ReturnsOkResponseWithItem()
        {
            var character = new Character { Id = 1, Title = "Hero", Desc = "Brave" };
            _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Character>())).ReturnsAsync(true);

            var request = new Request { Operation = RequestType.Update, Id = 1, Title = "Hero", Desc = "Brave" };

            var response = await _handler.Handle(request);

            Assert.Equal("ok", response.Status);
            Assert.Equal("Character updated", response.Message);
            Assert.NotNull(response.Item);
            Assert.Equal(1, response.Item!.Id);
            Assert.Null(response.Items);
        }

        [Fact]
        public async Task Handle_WhenUpdateAndIdDoesNotExist_ReturnsErrorResponse()
        {
            _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Character>())).ReturnsAsync(false);

            var request = new Request { Operation = RequestType.Update, Id = 1, Title = "Hero", Desc = "Brave" };

            var response = await _handler.Handle(request);

            Assert.Equal("error", response.Status);
            Assert.Equal("Id not present", response.Message);
            Assert.Null(response.Item);
            Assert.Null(response.Items);
        }

        [Fact]
        public async Task Handle_WhenUpdateAndIdIsNull_ReturnsErrorResponse()
        {
            var request = new Request { Operation = RequestType.Update, Id = null, Title = "Hero", Desc = "Brave" };

            var response = await _handler.Handle(request);

            Assert.Equal("error", response.Status);
            Assert.Equal("Id is required", response.Message);
            Assert.Null(response.Item);
            Assert.Null(response.Items);
        }

        // --- DELETE ---
        [Fact]
        public async Task Handle_WhenDeleteAndIdExists_ReturnsOkResponse()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var request = new Request { Operation = RequestType.Delete, Id = 1 };

            var response = await _handler.Handle(request);

            Assert.Equal("ok", response.Status);
            Assert.Equal("Character deleted", response.Message);
            Assert.Null(response.Item);
            Assert.Null(response.Items);
        }

        [Fact]
        public async Task Handle_WhenDeleteAndIdDoesNotExist_ReturnsErrorResponse()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

            var request = new Request { Operation = RequestType.Delete, Id = 1 };

            var response = await _handler.Handle(request);

            Assert.Equal("error", response.Status);
            Assert.Equal("Id not present", response.Message);
            Assert.Null(response.Item);
            Assert.Null(response.Items);
        }

        [Fact]
        public async Task Handle_WhenDeleteAndIdIsNull_ReturnsErrorResponse()
        {
            var request = new Request { Operation = RequestType.Delete, Id = null };

            var response = await _handler.Handle(request);

            Assert.Equal("error", response.Status);
            Assert.Equal("Id is required", response.Message);
            Assert.Null(response.Item);
            Assert.Null(response.Items);
        }

        // --- UNKNOWN ---
        [Fact]
        public async Task Handle_WhenUnknownOperation_ReturnsErrorResponse()
        {
            var request = new Request { Operation = (RequestType)999 };

            var response = await _handler.Handle(request);

            Assert.Equal("error", response.Status);
            Assert.Equal("Unknown operation", response.Message);
            Assert.Null(response.Item);
            Assert.Null(response.Items);
        }
    }
}
