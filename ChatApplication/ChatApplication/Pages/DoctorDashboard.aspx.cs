using System;
using System.Web.UI;

namespace ChatApplication.Pages
{
    public partial class DoctorDashboard : System.Web.UI.Page
    {
        /// <summary>
        /// Sayfa yüklendiğinde kullanıcı oturumu kontrol edilir ve hoş geldin mesajı gösterilir.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Eğer kullanıcı oturumu varsa hoş geldin mesajı gösterilir
            if (Session["Username"] != null)
            {
                lblWelcome.Text = "Hoş geldin, Dr. " + Session["Username"].ToString();
            }
            else
            {
                // Kullanıcı giriş yapmamışsa login sayfasına yönlendirilir
                Response.Redirect("Login.aspx");
            }
        }

        /// <summary>
        /// "Sohbete Git" butonuna tıklanınca sohbet sayfasına yönlendirme yapılır.
        /// </summary>
        protected void btnGoToChat_Click(object sender, EventArgs e)
        {
            Response.Redirect("Chat.aspx");
        }

        /// <summary>
        /// "Çıkış Yap" butonuna basıldığında oturum temizlenir ve login sayfasına dönülür.
        /// </summary>
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear(); // Tüm oturum verilerini temizle
            Response.Redirect("Login.aspx"); // Giriş sayfasına yönlendir
        }
    }
}
