<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PatientDashboard.aspx.cs" Inherits="ChatApplication.Pages.PatientDashboard" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Hasta Paneli</title>

    <style>
        /* Sayfa genel ayarları */
        body {
            margin: 0;
            padding: 0;
            background-color: #ece5dd; /* WhatsApp arka plan tonu */
            font-family: 'Segoe UI', sans-serif;
        }

        /* Panel kutusu */
        .dashboard-container {
            width: 400px;
            margin: 100px auto;
            background-color: white;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
            padding: 30px;
            text-align: center;
        }

        /* Başlık */
        h2 {
            color: #075e54; /* WhatsApp yeşili */
            margin-bottom: 20px;
        }

        /* Hoş geldiniz etiketi */
        .welcome-label {
            font-size: 18px;
            color: #333;
            margin-bottom: 30px;
            display: block;
        }

        /* Genel buton stilleri */
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

        /* Sohbet butonu */
        .btn-chat {
            background-color: #25d366; /* WhatsApp buton rengi */
            color: white;
        }

        .btn-chat:hover {
            background-color: #1ebc57;
        }

        /* Çıkış butonu */
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
        <!-- Panel Kutusu -->
        <div class="dashboard-container">
            <h2>Hasta Paneli</h2>

            <!-- Kullanıcıya hoş geldiniz mesajı -->
            <asp:Label ID="lblWelcome" runat="server" CssClass="welcome-label" />

            <!-- Sohbete Git Butonu -->
            <asp:Button ID="btnGoToChat" runat="server" Text="Sohbete Git" CssClass="btn btn-chat" OnClick="btnGoToChat_Click" />

            <!-- Çıkış Yap Butonu -->
            <asp:Button ID="btnLogout" runat="server" Text="Çıkış Yap" CssClass="btn btn-logout" OnClick="btnLogout_Click" />
        </div>
    </form>
</body>
</html>
