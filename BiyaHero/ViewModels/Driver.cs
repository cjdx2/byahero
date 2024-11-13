﻿using SQLite;

namespace BiyaHero.Models
{
    public class Driver
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string VehicleType { get; set; }
        public string VehiclePlateNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
