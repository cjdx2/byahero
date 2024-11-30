using BiyaHero.Services;

namespace BiyaHero;

public partial class TransactionHistoryPage : ContentPage
{
    private readonly DatabaseService _databaseService;

    public TransactionHistoryPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        LoadTransactionHistory();
    }

    private async void LoadTransactionHistory()
    {
        try
        {
            // Fetch trips from the database
            var trips = await _databaseService.GetTripsAsync();

            // Bind the trips to the CollectionView
            TransactionsCollectionView.ItemsSource = trips;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading transaction history: {ex.Message}");
            await DisplayAlert("Error", "Failed to load transaction history.", "OK");
        }
    }
}
