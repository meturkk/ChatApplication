using System;
using System.Web.UI;
using System.Data;
using System.Data.OleDb;

namespace ChatApplication.Pages
{
    public partial class Login : System.Web.UI.Page
    {
        /// <summary>
        /// Sayfa yüklendiğinde çalışır. Şu an için ek bir işlem yapılmıyor.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            // İlk yükleme sırasında yapılacak işlem yok
        }

        /// <summary>
        /// Giriş butonuna tıklanırsa kullanıcı adı ve şifre kontrol edilir.
        /// </summary>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Kullanıcıdan alınan giriş bilgileri
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Veritabanında kullanıcıyı sorgulayan SQL ifadesi
            string query = "SELECT * FROM USERS WHERE Username = ? AND Password = ?";

            // Parametreler hazırlanır (OleDb sıralamaya duyarlıdır!)
            OleDbParameter[] parameters = {
                new OleDbParameter("?", username),
                new OleDbParameter("?", password)
            };

            // Sorgu çalıştırılır ve sonuçlar alınır
            DataTable dt = Database.GetData(query, parameters);

            // Eğer kullanıcı bulunduysa oturum bilgileri kaydedilir
            if (dt.Rows.Count == 1)
            {
                string fullName = dt.Rows[0]["FullName"].ToString();
                string role = dt.Rows[0]["Role"].ToString();
                string userId = dt.Rows[0]["UserID"].ToString();

                // Oturum bilgileri
                Session["UserID"] = userId;
                Session["Username"] = fullName;
                Session["Role"] = role;

                // Kullanıcının rolüne göre yönlendirme yapılır
                if (role == "Doctor")
                    Response.Redirect("DoctorDashboard.aspx");
                else
                    Response.Redirect("PatientDashboard.aspx");
            }
            else
            {
                // Hatalı giriş durumu
                lblMessage.Text = "Hatalı kullanıcı adı veya şifre!";
            }
        }
    }
}
