using Server.Repositories;
using Server.Sql;
using Shared.Models;

namespace Server.Services
{
    internal class CharacterService : ICharacterService
    {

        private readonly ICharacterRepository _repository;
        private readonly ISqlCommandWriter _sqlWriter;
        private readonly SemaphoreSlim _lock = new(1, 1);


        public CharacterService(ICharacterRepository repository, ISqlCommandWriter sqlWriter)
        {
            _repository = repository;
            _sqlWriter = sqlWriter;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _lock.WaitAsync();
            try
            {
                var result = _repository.Delete(id);

                await _sqlWriter.WriteAsync($"DELETE FROM Characters WHERE Id = {id}");

                return result;
            }
            finally { _lock.Release(); }
        }

        public async Task<Character> CreateAsync(Character character)
        {
            await _lock.WaitAsync();
            try
            {
                var result = _repository.Create(character);
                await _sqlWriter.WriteAsync($"INSERT INTO Characters (Id, Title, Desc) VALUES ({character.Id}, '{character.Title}', '{character.Desc}');");
                return result;

            }
            finally { _lock.Release(); }
        }

        public async Task<IEnumerable<Character>> GetAllAsync()
        {
            await _lock.WaitAsync();
            try
            {
                var result = _repository.GetAll();
                await _sqlWriter.WriteAsync($"SELECT * FROM Characters;");
                return result;

            }
            finally { _lock.Release(); }
        }

        public async Task<Character?> GetByIdAsync(int id)
        {
            await _lock.WaitAsync();
            try
            {
                var result = _repository.GetById(id);

                await _sqlWriter.WriteAsync($"SELECT * FROM Characters WHERE Id = {id};");
                return result;

            }
            finally { _lock.Release(); }
        }

        public async Task<bool> UpdateAsync(Character character)
        {
            await _lock.WaitAsync();
            try
            {
                var result = _repository.Update(character);

                await _sqlWriter.WriteAsync($"UPDATE Characters SET Title = '{character.Title}', Desc = '{character.Desc}' WHERE Id = {character.Id};");

                return result;

            }
            finally { _lock.Release(); }
        }


    }

}
