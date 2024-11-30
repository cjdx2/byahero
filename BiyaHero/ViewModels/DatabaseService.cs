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
                _database.CreateTableAsync<Trip>().Wait();


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

        public Task<User> GetCurrentUserAsync()
        {
            return _database.Table<User>().FirstOrDefaultAsync();
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

        public Task<List<Report>> GetAllReportsAsync()
        {
            return _database.Table<Report>().ToListAsync();
        }

        public Task<Report> GetReportByIdAsync(int id)
        {
            return _database.Table<Report>().Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<Report>> GetReportsByUserEmailAsync(string email)
        {
            return _database.Table<Report>().Where(r => r.UserEmail == email).ToListAsync();
        }

        public Task<int> DeleteReportAsync(Report report)
        {
            return _database.DeleteAsync(report);
        }

        public Task<int> UpdateReportAsync(Report report)
        {
            return _database.UpdateAsync(report);
        }

        // Update report status (e.g., Resolved or Spam)
        public async Task<int> UpdateReportStatusAsync(string email, string status)
        {
            try
            {
                // Find all reports for the given email
                var reports = await GetReportsByUserEmailAsync(email);

                if (reports != null && reports.Count > 0)
                {
                    foreach (var report in reports)
                    {
                        report.Status = status; // Update status
                        await _database.UpdateAsync(report); // Save changes
                    }
                    return reports.Count; // Return the number of updated reports
                }

                return 0; // No reports found for the email
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating report status: {ex.Message}");
                return -1; // Return a failure code
            }
        }

        // Fetch reports by status (Resolved or Spam)
        public Task<List<Report>> GetReportsByStatusAsync(string status)
        {
            return _database.Table<Report>().Where(r => r.Status == status).ToListAsync();
        }

                // Insert a trip record
        public Task<int> AddTripAsync(Trip trip)
        {
            return _database.InsertAsync(trip);
        }

        // Get all trip records
        public Task<List<Trip>> GetTripsAsync()
        {
            return _database.Table<Trip>().ToListAsync();
        }


    }
}
