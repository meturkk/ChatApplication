using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChatApplication.Pages
{
    public partial class Chat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //  Oturum kontrolü
            if (Session["UserID"] == null || Session["Role"] == null)
            {
                Response.Write("Oturum açılmamış. Lütfen giriş yapınız.");
                Response.End();
            }

            if (!IsPostBack)
            {
                //  Kullanıcı listesini yükle
                LoadUserList();

                //  Eğer sohbet partneri seçilmemişse ilk kullanıcıya yönlendir
                if (Request.QueryString["to"] == null && rptUsers.Items.Count > 0)
                {
                    var firstUser = rptUsers.Items[0].FindControl("hfUserID") as HiddenField;
                    if (firstUser != null)
                    {
                        string firstUserId = firstUser.Value;
                        Response.Redirect("Chat.aspx?to=" + firstUserId);
                        return;
                    }
                }

                // Sohbet partnerinin rolü kontrol edilir (aynı rolse engellenir)
                if (Request.QueryString["to"] != null)
                {
                    string currentUserRole = Session["Role"].ToString();
                    string toUserId = Request.QueryString["to"];
                    if (!IsValidChatPartner(currentUserRole, toUserId))
                    {
                        Response.Write("❌ Aynı roldeki kullanıcılarla mesajlaşamazsınız.");
                        Response.End();
                    }
                }

                // Sohbet geçmişi yüklenir
                LoadChatHistory();
            }
        }

        
        // Kullanıcının karşıt roldeki kişileri listeler (Doktor → Hastaları, Hasta → Doktorları)
        private void LoadUserList()
        {
            string currentUserId = Session["UserID"].ToString();
            string currentUserRole = Session["Role"].ToString();
            string targetRole = currentUserRole == "Doctor" ? "Patient" : "Doctor";

            using (OleDbConnection connection = new OleDbConnection(GetConnectionString()))
            {
                string query = "SELECT UserID, Username FROM USERS WHERE Role = ? AND UserID <> ?";
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("?", targetRole);
                command.Parameters.AddWithValue("?", currentUserId);

                connection.Open();
                OleDbDataReader reader = command.ExecuteReader();

                rptUsers.DataSource = reader;
                rptUsers.DataBind();
            }
        }
         
        // Kullanıcı ile seçilen kişi arasındaki sohbet geçmişini yükler
        private void LoadChatHistory()
        {
            string currentUserId = Session["UserID"]?.ToString();
            string toUserId = Request.QueryString["to"];

            if (string.IsNullOrEmpty(currentUserId) || string.IsNullOrEmpty(toUserId))
            {
                literalMessages.Text = "Sohbet başlatılamadı. Giriş yapmadınız veya kullanıcı seçilmedi.";
                return;
            }

            string query = @"
                SELECT CHAT.*, USERS.Username 
                FROM CHAT 
                INNER JOIN USERS ON CHAT.SenderID = USERS.UserID 
                WHERE (SenderID = ? AND ReceiverID = ?) OR (SenderID = ? AND ReceiverID = ?) 
                ORDER BY Timestamp";

            var parameters = new OleDbParameter[]
            {
                new OleDbParameter("?", currentUserId),
                new OleDbParameter("?", toUserId),
                new OleDbParameter("?", toUserId),
                new OleDbParameter("?", currentUserId)
            };

            DataTable dt = Database.GetData(query, parameters);
            StringBuilder sb = new StringBuilder();

            //  Mesajlar sırayla işlenir
            foreach (DataRow row in dt.Rows)
            {
                string senderId = row["SenderID"].ToString();
                string senderUsername = row["Username"].ToString();
                string message = row["MessageText"].ToString();
                string time = Convert.ToDateTime(row["Timestamp"]).ToString("HH:mm");
                bool isRead = Convert.ToBoolean(row["IsRead"]);

                bool isSender = senderId == currentUserId;
                string cssClass = isSender ? "sent" : "received";
                string displayName = isSender ? "Ben" : senderUsername;
                string status = isSender
                    ? (isRead ? "<span style='color:green'>(Okundu)</span>" : "<span style='color:gray'>(Gönderildi)</span>")
                    : "";

                // HTML oluşturulup birleştirilir
                string html = $@"
                    <div class='message-bubble {cssClass}'>
                        <b>{displayName}</b><br />
                        {message}
                        <div class='timestamp'>{time} {status}</div>
                    </div>";

                sb.Append(html);
            }

            // Mesajlar Literal kontrolüne yazılır
            literalMessages.Text = sb.ToString();

            // Gelen ama okunmamış mesajlar "okundu" olarak işaretlenir
            string updateQuery = "UPDATE CHAT SET IsRead = true WHERE ReceiverID = ? AND SenderID = ? AND IsRead = false";
            var updateParams = new OleDbParameter[]
            {
                new OleDbParameter("?", currentUserId),
                new OleDbParameter("?", toUserId)
            };
            Database.ExecuteNonQuery(updateQuery, updateParams);
        }

        // Aynı roldeki kullanıcılarla sohbeti engellemek için kontrol sağlar
        private bool IsValidChatPartner(string currentRole, string partnerUserId)
        {
            string partnerRole = GetRole(partnerUserId);
            return currentRole != partnerRole;
        }

        // Verilen kullanıcı ID'sinin rolünü döndürür
        private string GetRole(string userId)
        {
            using (OleDbConnection conn = new OleDbConnection(GetConnectionString()))
            {
                string query = "SELECT Role FROM USERS WHERE UserID = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", userId);
                conn.Open();

                object result = cmd.ExecuteScalar();
                return result?.ToString() ?? "";
            }
        }

       
        // Veritabanı bağlantı cümlesi
        private string GetConnectionString()
        {
            return @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\chat_db.accdb;";
        }

        
        // Geri dön butonuna basıldığında kullanıcı rolüne göre dashboard sayfasına yönlendirir
        protected void btnBackToDashboard_Click(object sender, EventArgs e)
        {
            string role = Session["Role"]?.ToString();
            if (role == "Doctor")
                Response.Redirect("DoctorDashboard.aspx");
            else if (role == "Patient")
                Response.Redirect("PatientDashboard.aspx");
            else
                Response.Redirect("Login.aspx");
        }
    }
}
