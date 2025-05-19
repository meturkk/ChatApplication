<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ChatApplication.Pages.Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Giriş Yap</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <style>
        :root {
            --primary-color: #25d366;
            --primary-hover: #1ebc57;
            --secondary-color: #128C7E;
            --background-color: #f0f2f5;
            --card-color: #ffffff;
            --text-color: #333333;
            --border-color: #dddfe2;
            --error-color: #ff3333;
        }

        body {
            background-color: var(--background-color);
            font-family: 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
            margin: 0;
            padding: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            color: var(--text-color);
        }

        .login-container {
            width: 100%;
            max-width: 400px;
            background-color: var(--card-color);
            border-radius: 12px;
            padding: 2rem;
            text-align: center;
            box-shadow: 0 8px 24px rgba(0, 0, 0, 0.1);
            margin: 1rem;
        }

        .login-logo {
            width: 80px;
            height: 80px;
            margin-bottom: 1.5rem;
            object-fit: contain;
        }

        h2 {
            color: var(--primary-color);
            margin-bottom: 1.5rem;
            font-size: 1.8rem;
        }

        .input-group {
            margin-bottom: 1.5rem;
            text-align: center;
        }

        .input-label {
            display: block;
            margin-bottom: 0.5rem;
            font-weight: 500;
            color: var(--text-color);
            text-align: left;
            padding-left: 0.5rem;
        }

        .input-text {
            width: calc(100% - 2rem); /* Padding için alan bırakıyoruz */
            padding: 0.8rem 1rem;
            border: 1px solid var(--border-color);
            border-radius: 8px;
            font-size: 1rem;
            transition: border-color 0.3s, box-shadow 0.3s;
            margin: 0 auto;
            display: block;
            box-sizing: border-box;
        }

        .input-text:focus {
            outline: none;
            border-color: var(--primary-color);
            box-shadow: 0 0 0 2px rgba(37, 211, 102, 0.2);
        }

        .btn-login {
            background-color: var(--primary-color);
            color: white;
            border: none;
            padding: 0.8rem 1.5rem;
            margin: 1.5rem auto 0;
            border-radius: 8px;
            cursor: pointer;
            font-size: 1rem;
            font-weight: 600;
            transition: background-color 0.3s, transform 0.2s;
            width: 100%;
            max-width: 300px;
            display: block;
        }

        .btn-login:hover {
            background-color: var(--primary-hover);
            transform: translateY(-2px);
        }

        .btn-login:active {
            transform: translateY(0);
        }

        .error-message {
            color: var(--error-color);
            margin: 1rem 0;
            font-size: 0.9rem;
            display: block;
            text-align: center;
        }

        .register-link {
            margin-top: 1.5rem;
            display: block;
            color: var(--text-color);
            font-size: 0.9rem;
            text-align: center;
        }

        .register-link a {
            color: var(--secondary-color);
            text-decoration: none;
            font-weight: 500;
        }

        .register-link a:hover {
            text-decoration: underline;
        }

        @media (max-width: 480px) {
            .login-container {
                padding: 1.5rem;
                margin: 1rem;
                border-radius: 8px;
            }
            
            h2 {
                font-size: 1.5rem;
            }
            
            .input-text {
                width: calc(100% - 1rem);
            }
            
            .btn-login {
                max-width: 100%;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <!-- Logo -->
            <img src="logo.png" alt="Logo" class="login-logo" />

            <!-- Başlık -->
            <h2>Hesabınıza Giriş Yapın</h2>

            <!-- Hata mesajı -->
            <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />

            <!-- Kullanıcı Adı -->
            <div class="input-group">
                <label class="input-label" for="txtUsername">Kullanıcı Adı</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="input-text" placeholder="Kullanıcı adınızı girin" />
            </div>

            <!-- Şifre -->
            <div class="input-group">
                <label class="input-label" for="txtPassword">Şifre</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="input-text" placeholder="Şifrenizi girin" />
            </div>

            <!-- Giriş Butonu -->
            <asp:Button ID="btnLogin" runat="server" Text="Giriş Yap" CssClass="btn-login" OnClick="btnLogin_Click" />

            <!-- Kayıt Ol Linki -->
            <div class="register-link">
                Hesabınız yok mu? <a href="Register.aspx">Kayıt Olun</a>
            </div>
        </div>
    </form>
</body>
</html>