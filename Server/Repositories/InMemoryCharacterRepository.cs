using Server.Models;

namespace Server.Repositories
{

    /// <summary>
    /// In-memory repository for Character entities.
    /// </summary>
    /// <remarks>
    /// IMPORTANT: This class is NOT thread-safe by itself.
    /// Thread safety is enforced at the CharacterService level.
    /// </remarks>
    internal class InMemoryCharacterRepository : ICharacterRepository
    {

        private readonly Dictionary<int, Character> _storage = [];
        private int _nextId = 0;
        private readonly object _lock = new();

        public Character Create(Character character)
        {

            character.Id = _nextId;
            _storage[_nextId] = character;
            _nextId++;
            return character;

        }

        public bool Delete(int id)
        {

            if (_storage.ContainsKey(id))
            {
                _storage.Remove(id);
                return true;
            }

            return false;
        }

        public IEnumerable<Character> GetAll()
        {

            return _storage.Values.ToList();

        }

        public Character? GetById(int id)
        {

            if (_storage.TryGetValue(id, out Character? value))
            {
                return value;
            }

            return null;
        }

        public bool Update(Character character)
        {

            if (_storage.ContainsKey(character.Id))
            {
                _storage[character.Id] = character;
                return true;
            }

            return false;
        }
    }
}
