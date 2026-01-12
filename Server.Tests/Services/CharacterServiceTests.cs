using Moq;
using Server.Repositories;
using Server.Services;
using Server.Sql;
using Shared.Models;

namespace Server.Tests.Services
{
    public class CharacterServiceTests
    {
        private readonly Mock<ICharacterRepository> _repoMock;
        private readonly Mock<ISqlCommandWriter> _sqlWriterMock;
        private readonly CharacterService _service;

        public CharacterServiceTests()
        {
            _repoMock = new Mock<ICharacterRepository>();
            _sqlWriterMock = new Mock<ISqlCommandWriter>();
            _service = new CharacterService(_repoMock.Object, _sqlWriterMock.Object);
        }

        // --- CREATE ---
        [Fact]
        public async Task CreateAsync_WhenCharacterIsValid_ReturnsCreatedCharacter()
        {
            var character = new Character { Id = 1, Title = "Hero", Desc = "Brave" };
            _repoMock.Setup(r => r.Create(character)).Returns(character);
            _sqlWriterMock.Setup(s => s.WriteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(character);

            Assert.Equal(character, result);
            _repoMock.Verify(r => r.Create(character), Times.Once);
            _sqlWriterMock.Verify(s => s.WriteAsync($"INSERT INTO Characters (Id, Title, Desc) VALUES ({character.Id}, '{character.Title}', '{character.Desc}');"), Times.Once);
        }

        // --- DELETE ---
        [Fact]
        public async Task DeleteAsync_WhenCharacterExists_ReturnsTrue()
        {
            int id = 42;
            _repoMock.Setup(r => r.Delete(id)).Returns(true);
            _sqlWriterMock.Setup(s => s.WriteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(id);

            Assert.True(result);
            _repoMock.Verify(r => r.Delete(id), Times.Once);
            _sqlWriterMock.Verify(s => s.WriteAsync($"DELETE FROM Characters WHERE Id = {id}"), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenCharacterDoesNotExist_ReturnsFalse()
        {
            int id = 42;
            _repoMock.Setup(r => r.Delete(id)).Returns(false);
            _sqlWriterMock.Setup(s => s.WriteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(id);

            Assert.False(result);
            _repoMock.Verify(r => r.Delete(id), Times.Once);
            _sqlWriterMock.Verify(s => s.WriteAsync($"DELETE FROM Characters WHERE Id = {id}"), Times.Once);
        }

        // --- GETALL ---
        [Fact]
        public async Task GetAllAsync_WhenRepositoryHasCharacters_ReturnsAllCharacters()
        {
            var characters = new List<Character> { new Character { Id = 1, Title = "Hero" } };
            _repoMock.Setup(r => r.GetAll()).Returns(characters);
            _sqlWriterMock.Setup(s => s.WriteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            var result = await _service.GetAllAsync();

            Assert.Equal(characters, result);
            _repoMock.Verify(r => r.GetAll(), Times.Once);
            _sqlWriterMock.Verify(s => s.WriteAsync("SELECT * FROM Characters;"), Times.Once);
        }

        // --- GETBYID ---
        [Fact]
        public async Task GetByIdAsync_WhenCharacterExists_ReturnsCharacter()
        {
            var character = new Character { Id = 1, Title = "Hero" };
            _repoMock.Setup(r => r.GetById(1)).Returns(character);
            _sqlWriterMock.Setup(s => s.WriteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            var result = await _service.GetByIdAsync(1);

            Assert.Equal(character, result);
            _repoMock.Verify(r => r.GetById(1), Times.Once);
            _sqlWriterMock.Verify(s => s.WriteAsync("SELECT * FROM Characters WHERE Id = 1;"), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCharacterDoesNotExist_ReturnsNull()
        {
            _repoMock.Setup(r => r.GetById(99)).Returns((Character?)null);
            _sqlWriterMock.Setup(s => s.WriteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            var result = await _service.GetByIdAsync(99);

            Assert.Null(result);
            _repoMock.Verify(r => r.GetById(99), Times.Once);
            _sqlWriterMock.Verify(s => s.WriteAsync("SELECT * FROM Characters WHERE Id = 99;"), Times.Once);
        }

        // --- UPDATE ---
        [Fact]
        public async Task UpdateAsync_WhenCharacterExists_ReturnsTrue()
        {
            var character = new Character { Id = 1, Title = "Hero", Desc = "Brave" };
            _repoMock.Setup(r => r.Update(character)).Returns(true);
            _sqlWriterMock.Setup(s => s.WriteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            var result = await _service.UpdateAsync(character);

            Assert.True(result);
            _repoMock.Verify(r => r.Update(character), Times.Once);
            _sqlWriterMock.Verify(s => s.WriteAsync($"UPDATE Characters SET Title = '{character.Title}', Desc = '{character.Desc}' WHERE Id = {character.Id};"), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenCharacterDoesNotExist_ReturnsFalse()
        {
            var character = new Character { Id = 1, Title = "Hero", Desc = "Brave" };
            _repoMock.Setup(r => r.Update(character)).Returns(false);
            _sqlWriterMock.Setup(s => s.WriteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            var result = await _service.UpdateAsync(character);

            Assert.False(result);
            _repoMock.Verify(r => r.Update(character), Times.Once);
            _sqlWriterMock.Verify(s => s.WriteAsync($"UPDATE Characters SET Title = '{character.Title}', Desc = '{character.Desc}' WHERE Id = {character.Id};"), Times.Once);
        }
    }

}
