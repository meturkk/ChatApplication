<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DoctorDashboard.aspx.cs" Inherits="ChatApplication.Pages.DoctorDashboard" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Doktor Paneli</title>

    <style>
        /* Sayfa arka planı ve genel font ayarları */
        body {
            margin: 0;
            padding: 0;
            background-color: #ece5dd;
            font-family: 'Segoe UI', sans-serif;
        }

        /* Paneli ortalamak ve tasarımı güzelleştirmek için container */
        .dashboard-container {
            width: 400px;
            margin: 100px auto;
            background-color: white;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
            padding: 30px;
            text-align: center;
        }

        /* Başlık rengi ve boşluk ayarları */
        h2 {
            color: #075e54;
            margin-bottom: 20px;
        }

        /* Hoş geldin mesajı stil ayarları */
        .welcome-label {
            font-size: 18px;
            color: #333;
            margin-bottom: 30px;
            display: block;
        }

        /* Genel buton stili */
        .btn {
            display: inline-block;
            padding: 10px 25px;
            margin: 10px 5px;
            font-size: 16px;
            border: none;
            border-radius: 30px;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

        /* Sohbet butonuna özel stil */
        .btn-chat {
            background-color: #25d366;
            color: white;
        }

        .btn-chat:hover {
            background-color: #1ebc57;
        }

        /* Çıkış butonuna özel stil */
        .btn-logout {
            background-color: #ff4d4d;
            color: white;
        }

        .btn-logout:hover {
            background-color: #e04444;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Dashboard paneli -->
        <div class="dashboard-container">
            <h2>Doktor Paneli</h2>

            <!-- Giriş yapan kullanıcıya hoş geldin mesajı -->
            <asp:Label ID="lblWelcome" runat="server" CssClass="welcome-label" />

            <!-- Sohbet ekranına yönlendirme butonu -->
            <asp:Button ID="btnGoToChat" runat="server" Text="Sohbete Git" CssClass="btn btn-chat" OnClick="btnGoToChat_Click" />

            <!-- Oturumu sonlandırma butonu -->
            <asp:Button ID="btnLogout" runat="server" Text="Çıkış Yap" CssClass="btn btn-logout" OnClick="btnLogout_Click" />
        </div>
    </form>
</body>
</html>
