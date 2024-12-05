using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiyaHero.Models {
    public class BookingData {
        public double originLat { get; set; }
        public double originLong { get; set; }
        public double destinationLat { get; set; }
        public double destinationLong { get; set; }
        public string originAddress { get; set; }
        public string destinationAddress { get; set; }
        public string riderName { get; set; }
        public string uuid { get; set; }
        public string type { get; set; }
        public double distance { get; set; }
        public double price { get; set; }
        public string paymentMethod { get; set; }
        public DriverData? driver { get; set; }
    }

    public class DriverData {
        public double currentLat { get; set; }
        public double currentLong { get; set; }
    }

    public class ChatData {
        public string sender { get; set; }
        public string message { get; set; }
        public Color textColor { get; set; }
        public TextAlignment textAlign { get; set; }
        public Color backgroundColor { get; set; }
        public LayoutOptions horizontalOptions { get; set; }
        public Thickness margin { get; set; }
    }

}
