using Server.Models;

namespace Server.Repositories
{
    internal class InMemoryCharacterRepository : ICharacterRepository
    {

        private readonly Dictionary<int, Character> _storage = [];
        private int _nextId = 0;
        private readonly object _lock = new();

        public Character Create(Character character)
        {
            lock (_lock)
            {
                character.Id = _nextId;
                _storage[_nextId] = character;
                _nextId++;
                return character;
            }
        }

        public bool Delete(int id)
        {
            lock (_lock)
            {
                if (_storage.ContainsKey(id))
                {
                    _storage.Remove(id);
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<Character> GetAll()
        {
            lock (_lock)
            {
                return _storage.Values.ToList();
            }
        }

        public Character? GetById(int id)
        {
            lock (_lock)
            {
                if (_storage.TryGetValue(id, out Character? value))
                {
                    return value;
                }
            }
            return null;
        }

        public bool Update(Character character)
        {
            lock (_lock)
            {
                if (_storage.ContainsKey(character.Id))
                {
                    _storage[character.Id] = character;
                    return true;
                }
            }
            return false;
        }
    }
}
