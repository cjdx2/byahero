using SQLite;
using System.IO;
using System.Threading.Tasks;
using BiyaHero.Models;

namespace BiyaHero.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BiyaHero.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<Driver>().Wait();
        }

        public Task<int> AddUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return _database.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
        }
        public Task<int> AddDriverAsync(Driver driver)
        {
            return _database.InsertAsync(driver);
        }

        // Get a driver by email
        public Task<Driver> GetDriverByEmailAsync(string email)
        {
            return _database.Table<Driver>().Where(d => d.Email == email).FirstOrDefaultAsync();
        }
    }
}

