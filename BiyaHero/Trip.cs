using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;


namespace BiyaHero
{
    public class Trip
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string PaymentMethod { get; set; }
        public double Distance { get; set; }
        public double Price { get; set; }

    }
}
