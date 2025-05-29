using EngishMotherFucker.Models;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EngishMotherFucker.Database
{
    public class WordDatabase : IDisposable
    {
        private readonly SQLiteAsyncConnection _database;

        public WordDatabase()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "words.db");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<WordModel>().Wait(); // однократная синхронная инициализация
        }

        public Task<List<WordModel>> GetWordsAsync() =>
            _database.Table<WordModel>().ToListAsync();

        public Task<int> SaveWordAsync(WordModel word)
        {
            if (word.Id != 0)
                return _database.UpdateAsync(word);
            else
                return _database.InsertAsync(word);
        }

        public Task<int> DeleteWordAsync(WordModel word) =>
            _database.DeleteAsync(word);

        public Task<int> DeleteAllWordsAsync() =>
    _database.DeleteAllAsync<WordModel>();

        public void Dispose()
        {
            var conn = _database.GetConnection();
            conn.Close();
            conn.Dispose();
        }
    }
}
