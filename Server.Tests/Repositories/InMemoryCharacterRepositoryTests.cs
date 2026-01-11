using Server.Repositories;
using Shared.Models;

namespace Server.Tests.Repositories
{
    public class InMemoryCharacterRepositoryTests
    {
        [Fact]
        public void Create_WhenCalled_AssignsIncrementalId()
        {
            var repo = new InMemoryCharacterRepository();
            var character = new Character { Title = "Test" };

            var created = repo.Create(character);

            Assert.Equal(0, created.Id);
        }

        [Fact]
        public void GetById_WhenCharacterExists_ReturnsCharacter()
        {
            var repo = new InMemoryCharacterRepository();
            var created = repo.Create(new Character { Title = "Test" });

            var result = repo.GetById(created.Id);

            Assert.NotNull(result);
            Assert.Equal("Test", result!.Title);
        }

        [Fact]
        public void GetById_WhenCharacterDoesNotExist_ReturnsNull()
        {
            var repo = new InMemoryCharacterRepository();

            var result = repo.GetById(999);

            Assert.Null(result);
        }

        [Fact]
        public void Update_WhenCharacterExists_ReturnsTrue()
        {
            var repo = new InMemoryCharacterRepository();
            var created = repo.Create(new Character { Title = "Old" });

            created.Title = "New";
            var result = repo.Update(created);

            Assert.True(result);
            Assert.Equal("New", repo.GetById(created.Id)!.Title);
        }

        [Fact]
        public void Update_WhenCharacterDoesNotExist_ReturnsFalse()
        {
            var repo = new InMemoryCharacterRepository();
            var character = new Character { Id = 999, Title = "Ghost" };

            var result = repo.Update(character);

            Assert.False(result);
        }

        [Fact]
        public void Delete_WhenCharacterExists_ReturnsTrue()
        {
            var repo = new InMemoryCharacterRepository();
            var created = repo.Create(new Character { Title = "Test" });

            var result = repo.Delete(created.Id);

            Assert.True(result);
            Assert.Null(repo.GetById(created.Id));
        }

        [Fact]
        public void Delete_WhenCharacterDoesNotExist_ReturnsFalse()
        {
            var repo = new InMemoryCharacterRepository();

            var result = repo.Delete(999);

            Assert.False(result);
        }

        [Fact]
        public void GetAll_WhenCalled_ReturnsAllCharacters()
        {
            var repo = new InMemoryCharacterRepository();
            repo.Create(new Character { Title = "A" });
            repo.Create(new Character { Title = "B" });

            var all = repo.GetAll().ToList();

            Assert.Equal(2, all.Count);
        }
    }
}
