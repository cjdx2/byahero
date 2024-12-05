using BiyaHero.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;

namespace BiyaHero;

public partial class UserChat : ContentPage
{
	string uuid;
	FirebaseClient firebaseClient;

    public ObservableCollection<ChatData> ChatLog { get; set; }

    public UserChat(string id)
	{
		InitializeComponent();
        firebaseClient = new FirebaseClient("https://biyahero-7d8a3-default-rtdb.firebaseio.com");
        ChatLog = new ObservableCollection<ChatData>();
        uuid = id;
        BindingContext = this;
        ListenToChat();
    }

    public void ListenToChat() {
        firebaseClient
            .Child($"Chats/{uuid}")
            .AsObservable<ChatData>()
            .Subscribe(item => {
                if (item.Object != null) {
                    MainThread.BeginInvokeOnMainThread(async () => {
                        // Add the new chat to the ChatLog collection
                        var chatData = item.Object as ChatData;

                        if (chatData.sender == "rider") {
                            chatData.textColor = Color.FromHex("#000");
                            chatData.textAlign = TextAlignment.End;
                            chatData.backgroundColor = Color.FromHex("#d3d3d3");
                            chatData.horizontalOptions = LayoutOptions.End;
                            chatData.margin = new Thickness(40, 5, 10, 5);
                        } else if (chatData.sender == "driver") {
                            chatData.textColor = Color.FromHex("#fff");
                            chatData.textAlign = TextAlignment.Start;
                            chatData.backgroundColor = Color.FromHex("#000");
                            chatData.horizontalOptions = LayoutOptions.Start;
                            chatData.margin = new Thickness(10, 5, 40, 5);
                        }

                        ChatLog.Add(chatData);
                    });
                }
            });
    }

	private async void OnSendClicked(object sender, EventArgs e) {
        string dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");

		var data = new ChatData {
			sender = "rider",
			message = MessageEntry.Text
		};

		await firebaseClient.Child("Chats").Child(uuid).Child(dateString).PutAsync(data);
	}
}