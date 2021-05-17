using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using AppTFG.Modelos;

namespace AppTFG.BaseDeDatosLocal
{
    public class LocalDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public LocalDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Saltar>().Wait();
        }

        public Task<List<Saltar>> GetSaltarsAsync()
        {
            return _database.Table<Saltar>().ToListAsync();
        }

        public Task<Saltar> GetSaltarAsync(int id)
        {
            return _database.Table<Saltar>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveSaltarAsync(Saltar note)
        {
            if (note.ID != 0)
            {
                return _database.UpdateAsync(note);
            }
            else
            {
                return _database.InsertAsync(note);
            }
        }

        public Task<int> DeleteAllSaltarAsync()
        {
            return _database.DeleteAllAsync<Saltar>();
        }
    }
}
