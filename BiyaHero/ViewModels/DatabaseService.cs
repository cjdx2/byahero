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

        public Task<List<User>> GetAllUsersAsync()
        {
            return _database.Table<User>().ToListAsync();
        }

        public Task<int> AddUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return _database.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public Task<int> DeleteUserAsync(User user)
        {
            return _database.DeleteAsync(user);
        }

        public Task<int> UpdateUserAsync(User user)
        {
            return _database.UpdateAsync(user);
        }
        public Task<int> AddDriverAsync(Driver driver)
        {
            return _database.InsertAsync(driver);
        }

        // Fetch all drivers
        public Task<List<Driver>> GetAllDriversAsync()
        {
            return _database.Table<Driver>().ToListAsync();
        }

        // Get a driver by email
        public Task<Driver> GetDriverByEmailAsync(string email)
        {
            return _database.Table<Driver>().Where(d => d.Email == email).FirstOrDefaultAsync();
        }

        // Delete a driver
        public Task<int> DeleteDriverAsync(Driver driver)
        {
            return _database.DeleteAsync(driver);
        }

        // Update driver details
        public Task<int> UpdateDriverAsync(Driver driver)
        {
            return _database.UpdateAsync(driver);
        }
    }
}