using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Data.OleDb;

public class ChatHub : Hub
{
    // Kullanıcı bağlantılarını saklayan sözlük: UserID → ConnectionID
    public static ConcurrentDictionary<string, string> ConnectedUsers = new ConcurrentDictionary<string, string>();

    // Kullanıcı bağlandığında çağrılır
    public override System.Threading.Tasks.Task OnConnected()
    {
        var userId = Context.QueryString["userid"];
        if (!string.IsNullOrEmpty(userId))
        {
            // Kullanıcıyı sözlüğe ekle veya güncelle
            ConnectedUsers[userId] = Context.ConnectionId;
        }
        return base.OnConnected();
    }

    // Kullanıcı bağlantısı kesildiğinde çağrılır
    public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
    {
        var userId = Context.QueryString["userid"];
        if (!string.IsNullOrEmpty(userId))
        {
            // Kullanıcıyı sözlükten çıkar
            ConnectedUsers.TryRemove(userId, out _);
        }
        return base.OnDisconnected(stopCalled);
    }

    // Kullanıcı mesaj gönderdiğinde çağrılır
    public void Send(string senderId, string receiverId, string message)
    {
        // Gönderenin adını veritabanından al
        string senderUsername = GetUsernameById(senderId);

        // Alıcı çevrimiçiyse mesajı sadece ona gönder
        if (ConnectedUsers.TryGetValue(receiverId, out string receiverConnId))
        {
            Clients.Client(receiverConnId).receiveMessage(senderId, receiverId, senderUsername, message);
        }

        // Mesajı veritabanına kaydet
        SaveMessageToDatabase(senderId, receiverId, message);
    }

    // Veritabanına mesaj ekler
    private void SaveMessageToDatabase(string senderId, string receiverId, string message)
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\chat_db.accdb";

        using (OleDbConnection connection = new OleDbConnection(connectionString))
        {
            string query = "INSERT INTO CHAT ([SenderId], [ReceiverId], [MessageText], [Timestamp], [IsRead]) VALUES (?, ?, ?, ?, ?)";
            OleDbCommand command = new OleDbCommand(query, connection);

            // Parametreleri sırayla eklemek zorundayız (OleDb böyle çalışır)
            command.Parameters.Add("SenderId", OleDbType.Integer).Value = Convert.ToInt32(senderId);
            command.Parameters.Add("ReceiverId", OleDbType.Integer).Value = Convert.ToInt32(receiverId);
            command.Parameters.Add("Message", OleDbType.LongVarWChar).Value = message;
            command.Parameters.Add("Timestamp", OleDbType.Date).Value = DateTime.Now;
            command.Parameters.Add("IsRead", OleDbType.Boolean).Value = false; // İlk başta okunmamış

            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    // UserID ile kullanıcı adını veritabanından getirir
    private string GetUsernameById(string userId)
    {
        string username = "Bilinmeyen";
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\chat_db.accdb;";
        using (OleDbConnection conn = new OleDbConnection(connectionString))
        {
            string query = "SELECT Username FROM USERS WHERE UserID = ?";
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("?", userId);
            conn.Open();

            var result = cmd.ExecuteScalar();
            if (result != null)
                username = result.ToString();
        }
        return username;
    }

    // Kullanıcı yazmaya başladığında "yazıyor..." bildirimi gönderir
    public void Typing(string senderId, string receiverId)
    {
        if (ConnectedUsers.TryGetValue(receiverId, out string connId))
        {
            string senderUsername = GetUsernameById(senderId);
            Clients.Client(connId).showTyping(senderUsername);
        }
    }
}
