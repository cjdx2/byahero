using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace BiyaHero
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Places { get; set; }
        public ObservableCollection<string> PaymentMethods { get; set; }


        private string _selectedFrom;
        public string SelectedFrom
        {
            get => _selectedFrom;
            set
            {
                _selectedFrom = value;
                OnPropertyChanged(nameof(SelectedFrom));
            }
        }
        private string _selectedTo;
        public string SelectedTo
        {
            get => _selectedTo;
            set
            {
                _selectedTo = value;
                OnPropertyChanged(nameof(SelectedTo));
            }
        }
        private string _distanceResult;
        public string DistanceResult
        {
            get => _distanceResult;
            set
            {
                _distanceResult = value;
                OnPropertyChanged(nameof(DistanceResult));
            }
        }
        private string _priceResult;
        public string PriceResult
        {
            get => _priceResult;
            set
            {
                _priceResult = value;
                OnPropertyChanged(nameof(PriceResult));
            }
        }


        private string _selectedPaymentMethod;
        public string SelectedPaymentMethod
        {
            get => _selectedPaymentMethod;
            set
            {
                _selectedPaymentMethod = value;
                OnPropertyChanged(nameof(SelectedPaymentMethod));
                UpdatePaymentMethodResult();
            }
        }

        private string _paymentMethodResult;
        public string PaymentMethodResult
        {
            get => _paymentMethodResult;
            set
            {
                _paymentMethodResult = value;
                OnPropertyChanged(nameof(PaymentMethodResult));
            }
        }

        // Command for the "Get Price" button
        public ICommand GetPriceCommand { get; }

        // Static distance data between places (miles)
        private readonly Dictionary<(string, string), double> _distances = new()
        {
            { ("Pamantasan ng Lungsod ng Maynila", "Gateway Mall Cubao"), 10.0 },
            { ("Pamantasan ng Lungsod ng Maynila", "Cartimar Shopping Center Pasay"), 5.7 },
            { ("Pamantasan ng Lungsod ng Maynila", "McDonald's Recto Rizal Ave"), 3.2 },
            { ("Pamantasan ng Lungsod ng Maynila", "Divisoria Mall"), 3.6 },
            { ("Gateway Mall Cubao", "Pamantasan ng Lungsod ng Maynila"), 10.0 },
            { ("Gateway Mall Cubao", "Cartimar Shopping Center Pasay"), 15.7 },
            { ("Gateway Mall Cubao", "McDonald's Recto Rizal Ave"), 9.4 },
            { ("Gateway Mall Cubao", "Divisoria Mall"), 11.3 },
            { ("Cartimar Shopping Center Pasay", "Divisoria Mall"), 8.1 },
            { ("Cartimar Shopping Center Pasay", "McDonald's Recto Rizal Ave"), 7.1 },
            { ("Cartimar Shopping Center Pasay", "Gateway Mall Cubao"), 15.7 },
            { ("Cartimar Shopping Center Pasay", "Pamantasan ng Lungsod ng Maynila"), 5.7 },
            { ("McDonald's Recto Rizal Ave", "Pamantasan ng Lungsod ng Maynila"), 3.2 },
            { ("McDonald's Recto Rizal Ave", "Gateway Mall Cubao"), 9.4 },
            { ("McDonald's Recto Rizal Ave", "Cartimar Shopping Center Pasay"), 7.1 },
            { ("McDonald's Recto Rizal Ave", "Divisoria Mall"), 2.6 },
            { ("Divisoria Mall", "Pamantasan ng Lungsod ng Maynila"), 3.6 },
            { ("Divisoria Mall", "Gateway Mall Cubao"), 11.3 },
            { ("Divisoria Mall", "Cartimar Shopping Center Pasay"), 8.1 },
            { ("Divisoria Mall", "McDonald's Recto Rizal Ave"), 2.6 },
        };



        // Price per mile (for example)
        private const double PricePerMile = 25.0;

        public MainPageViewModel()
        {
            Places = new ObservableCollection<string>
            {
                "Pamantasan ng Lungsod ng Maynila",
                "Gateway Mall Cubao",
                "Cartimar Shopping Center Pasay",
                "McDonald's Recto Rizal Ave",
                "Divisoria Mall"
            };

            PaymentMethods = new ObservableCollection<string>
            {
                "Credit Card",
                "PayPal",
                "Cash",
                "GCash"
            };

            // Initialize the Get Price command
            GetPriceCommand = new Command(UpdateDistanceAndPrice);

        }
        public void UpdateDistanceAndPrice()
        {
            if (!string.IsNullOrEmpty(SelectedFrom) && !string.IsNullOrEmpty(SelectedTo))
            {
                if (_distances.TryGetValue((SelectedFrom, SelectedTo), out double distance) ||
                    _distances.TryGetValue((SelectedTo, SelectedFrom), out distance)) // Check reverse key
                {
                    // Calculate the price
                    double price = distance * PricePerMile;
                    DistanceResult = $"Distance: {distance} kilometer.";
                    PriceResult = $"Price: ₱{price:F2}";
                }
                else
                {
                    DistanceResult = "Erro!";
                    PriceResult = "Error!";
                }
            }
            else
            {
                DistanceResult = "Please Select Location";
                PriceResult = "Please select Destination";
            }
        }

        private void UpdatePaymentMethodResult()
        {
            if (!string.IsNullOrEmpty(SelectedPaymentMethod))
            {
                PaymentMethodResult = $"Mode of Payment: {SelectedPaymentMethod}.";
            }
            else
            {
                PaymentMethodResult = "Please Select Mode of Payment";
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
