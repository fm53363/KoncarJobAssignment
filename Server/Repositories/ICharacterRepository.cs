using Server.Models;

namespace Server.Repositories
{
    interface ICharacterRepository
    {
        Character? GetById(int id);
        IEnumerable<Character> GetAll();
        Character Create(Character character);
        bool Update(Character character);

        bool Delete(int id);
    }
}
