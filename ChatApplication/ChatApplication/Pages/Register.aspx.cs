using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChatApplication.Pages
{
	public partial class Register : System.Web.UI.Page
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            // İlk yükleme sırasında yapılacak işlem yok
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            // Form verilerini al
            string fullName = txtFullName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string passwordConfirm = txtPasswordConfirm.Text.Trim();
            string role = ddlRole.SelectedValue;

            // Validasyonlar
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(passwordConfirm))
            {
                lblMessage.Text = "Tüm alanları doldurunuz!";
                return;
            }

            if (password != passwordConfirm)
            {
                lblMessage.Text = "Şifreler uyuşmuyor!";
                return;
            }

            // Kullanıcı adı kontrolü
            string checkUserQuery = "SELECT COUNT(*) FROM [USERS] WHERE Username = ?";
            OleDbParameter[] checkParams = { new OleDbParameter("?", username) };
            int userCount = Convert.ToInt32(Database.GetScalar(checkUserQuery, checkParams));

            if (userCount > 0)
            {
                lblMessage.Text = "Bu kullanıcı adı zaten alınmış!";
                return;
            }

            // Email kontrolü
            string checkEmailQuery = "SELECT COUNT(*) FROM [USERS] WHERE Email = ?";
            OleDbParameter[] emailParams = { new OleDbParameter("?", email) };
            int emailCount = Convert.ToInt32(Database.GetScalar(checkEmailQuery, emailParams));

            if (emailCount > 0)
            {
                lblMessage.Text = "Bu email adresi zaten kayıtlı!";
                return;
            }


            // Yeni kullanıcıyı kaydet
            string insertQuery = "INSERT INTO [USERS] ([Username], [Password], [Role], [FullName], [Email]) VALUES (@Username, @Password, @Role, @FullName, @Email)";

            OleDbParameter[] insertParams = {
                new OleDbParameter("@Username", OleDbType.VarChar) { Value = username },
                new OleDbParameter("@Password", OleDbType.VarChar) { Value = password },
                new OleDbParameter("@Role", OleDbType.VarChar) { Value = role },
                new OleDbParameter("@FullName", OleDbType.VarChar) { Value = fullName },
                new OleDbParameter("@Email", OleDbType.VarChar) { Value = email }
            };

            try
            {
                int rowsAffected = Database.ExecuteNonQuery(insertQuery, insertParams);
                if (rowsAffected > 0)
                {
                    lblMessage.Text = "";
                    lblSuccess.Text = "Kayıt başarılı! Giriş yapabilirsiniz.";
                    // Formu temizle
                    txtFullName.Text = "";
                    txtUsername.Text = "";
                    txtEmail.Text = "";
                    txtPassword.Text = "";
                    txtPasswordConfirm.Text = "";
                }
                else
                {
                    lblMessage.Text = "Kayıt sırasında bir hata oluştu!";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Hata: " + ex.Message;
            }
        }

        protected void btnGoToLogin_Click(object sender, EventArgs e)
        {
            // Login.aspx sayfasına yönlendir
            Response.Redirect("Login.aspx");
        }
    }
}