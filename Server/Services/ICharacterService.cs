using Shared.Models;

namespace Server.Services
{
    interface ICharacterService
    {
        Task<Character?> GetByIdAsync(int id);
        Task<IEnumerable<Character>> GetAllAsync();
        Task<Character> CreateAsync(Character character);
        Task<bool> UpdateAsync(Character character);
        Task<bool> DeleteAsync(int id);
    }
}
