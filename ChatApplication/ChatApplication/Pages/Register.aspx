<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ChatApplication.Pages.Register" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Üye Ol</title>
    <style>
        body {
            background-color: #ece5dd;
            font-family: 'Segoe UI', sans-serif;
            margin: 0;
            padding: 0;
        }
        .register-container {
            width: 400px;
            margin: 50px auto;
            background-color: #ffffff;
            border-radius: 10px;
            padding: 30px;
            text-align: center;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
        }
        .register-logo {
            width: 80px;
            height: 80px;
            margin-bottom: 20px;
        }
        h2 {
            color: #075e54;
            margin-bottom: 20px;
        }
        .input-label {
            display: block;
            text-align: left;
            margin-top: 10px;
            margin-bottom: 5px;
            font-weight: bold;
        }
        .input-text {
            width: 100%;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 20px;
            font-size: 14px;
        }
        .btn-register {
            background-color: #25d366;
            color: white;
            border: none;
            padding: 10px 20px;
            margin-top: 20px;
            border-radius: 30px;
            cursor: pointer;
            font-size: 16px;
            font-weight: bold;
            transition: background-color 0.3s;
        }
        .btn-register:hover {
            background-color: #1ebc57;
        }
        .btn-login {
            background-color: #128c7e;
            color: white;
            border: none;
            padding: 10px 20px;
            margin-top: 10px;
            border-radius: 30px;
            cursor: pointer;
            font-size: 16px;
            font-weight: bold;
            transition: background-color 0.3s;
            display: block;
            width: 100%;
        }
        .btn-login:hover {
            background-color: #0c6b5f;
        }
        .error-message {
            color: red;
            margin-top: 10px;
        }
        .success-message {
            color: green;
            margin-top: 10px;
        }
        .divider {
            margin: 20px 0;
            text-align: center;
            position: relative;
        }
        .divider:before {
            content: "";
            position: absolute;
            top: 50%;
            left: 0;
            width: 45%;
            height: 1px;
            background-color: #ccc;
        }
        .divider:after {
            content: "";
            position: absolute;
            top: 50%;
            right: 0;
            width: 45%;
            height: 1px;
            background-color: #ccc;
        }
        .divider-text {
            display: inline-block;
            padding: 0 10px;
            background-color: #fff;
            position: relative;
            z-index: 1;
            color: #777;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="register-container">
            <!-- Logo -->
            <img src="logo.png" alt="Logo" class="register-logo" />
            <!-- Başlık -->
            <h2>Yeni Üye Kaydı</h2>
            <!-- Hata mesajı -->
            <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
            <asp:Label ID="lblSuccess" runat="server" CssClass="success-message" />
            <!-- Tam Ad -->
            <label class="input-label" for="txtFullName">Tam Adınız:</label>
            <asp:TextBox ID="txtFullName" runat="server" CssClass="input-text" />
            <!-- Kullanıcı Adı -->
            <label class="input-label" for="txtUsername">Kullanıcı Adı:</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="input-text" />
            <!-- Email -->
            <label class="input-label" for="txtEmail">Email:</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="input-text" TextMode="Email" />
            <!-- Şifre -->
            <label class="input-label" for="txtPassword">Şifre:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="input-text" />
            <!-- Şifre Tekrar -->
            <label class="input-label" for="txtPasswordConfirm">Şifre Tekrar:</label>
            <asp:TextBox ID="txtPasswordConfirm" runat="server" TextMode="Password" CssClass="input-text" />
            <!-- Rol Seçimi -->
            <label class="input-label" for="ddlRole">Rolünüz:</label>
            <asp:DropDownList ID="ddlRole" runat="server" CssClass="input-text">
                <asp:ListItem Value="Patient">Hasta</asp:ListItem>
                <asp:ListItem Value="Doctor">Doktor</asp:ListItem>
            </asp:DropDownList>
            <!-- Kayıt Butonu -->
            <asp:Button ID="btnRegister" runat="server" Text="Kayıt Ol" CssClass="btn-register" OnClick="btnRegister_Click" />
            
            <!-- Ayırıcı -->
            <div class="divider">
                <span class="divider-text">veya</span>
            </div>
            
            <!-- Giriş Yap Butonu -->
            <asp:Button ID="btnGoToLogin" runat="server" Text="Giriş Yap" CssClass="btn-login" OnClick="btnGoToLogin_Click" />
        </div>
    </form>
</body>
</html>