using SQLite;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using BiyaHero.Models;

namespace BiyaHero.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            try
            {
                var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BiyaHero.db3");
                _database = new SQLiteAsyncConnection(dbPath);

                // Create tables for User, Driver, and Report if they don't exist already
                _database.CreateTableAsync<User>().Wait();
                _database.CreateTableAsync<Driver>().Wait();
                _database.CreateTableAsync<Report>().Wait();

                Console.WriteLine($"Database created at: {dbPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
            }
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

        // New method to fetch the current logged-in user (Modify according to your login mechanism)
        public Task<User> GetCurrentUserAsync()
        {
            // Assuming we have a method to retrieve the logged-in user by session or identifier.
            // Here, we assume that the first user is the logged-in user for demonstration purposes.
            return _database.Table<User>().FirstOrDefaultAsync();  // This can be adjusted based on your session management.
        }

        public Task<int> AddDriverAsync(Driver driver)
        {
            return _database.InsertAsync(driver);
        }

        public Task<List<Driver>> GetAllDriversAsync()
        {
            return _database.Table<Driver>().ToListAsync();
        }

        public Task<Driver> GetDriverByEmailAsync(string email)
        {
            return _database.Table<Driver>().Where(d => d.Email == email).FirstOrDefaultAsync();
        }

        public Task<int> DeleteDriverAsync(Driver driver)
        {
            return _database.DeleteAsync(driver);
        }

        public Task<int> UpdateDriverAsync(Driver driver)
        {
            return _database.UpdateAsync(driver);
        }

        // Add Report
        public async Task<int> AddReportAsync(Report report)
        {
            try
            {
                return await _database.InsertAsync(report);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving report: {ex.Message}");
                return -1; // Return a failure code
            }
        }

        // Get All Reports
        public Task<List<Report>> GetAllReportsAsync()
        {
            return _database.Table<Report>().ToListAsync();
        }

        // Get Report by ID
        public Task<Report> GetReportByIdAsync(int id)
        {
            return _database.Table<Report>().Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        // Get Reports by User Email
        public Task<List<Report>> GetReportsByUserEmailAsync(string email)
        {
            return _database.Table<Report>().Where(r => r.UserEmail == email).ToListAsync();
        }

        // Delete Report
        public Task<int> DeleteReportAsync(Report report)
        {
            return _database.DeleteAsync(report);
        }

        // Update Report
        public Task<int> UpdateReportAsync(Report report)
        {
            return _database.UpdateAsync(report);
        }
    }
}
