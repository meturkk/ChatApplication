using System;
using System.Web.UI;

namespace ChatApplication.Pages
{
    public partial class PatientDashboard : System.Web.UI.Page
    {
        // Sayfa yüklendiğinde çalışır
        protected void Page_Load(object sender, EventArgs e)
        {
            // Kullanıcı oturumu kontrolü
            if (Session["Username"] != null)
            {
                // Hoş geldiniz mesajı kullanıcı adına göre yazdırılır
                lblWelcome.Text = "Hoş geldin, " + Session["Username"].ToString();
            }
            else
            {
                // Giriş yapılmamışsa login sayfasına yönlendir
                Response.Redirect("Login.aspx");
            }
        }

        // Sohbete Git butonuna basıldığında çağrılır
        protected void btnGoToChat_Click(object sender, EventArgs e)
        {
            Response.Redirect("Chat.aspx"); // Sohbet sayfasına yönlendir
        }

        // Çıkış Yap butonuna basıldığında çağrılır
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear(); // Oturum bilgilerini temizle
            Response.Redirect("Login.aspx"); // Giriş sayfasına yönlendir
        }
    }
}
