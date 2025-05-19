<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="ChatApplication.Pages.Chat" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Gerçek Zamanlı Sohbet</title>

    <!-- jQuery ve SignalR scriptleri -->
    <script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
    <script src='<%= ResolveClientUrl("~/Scripts/jquery.signalR-2.4.2.min.js") %>'></script>
    <script src="/signalr/hubs"></script>

    <!-- Emoji picker eklentisi -->
    <script src="https://cdn.jsdelivr.net/npm/@joeattardi/emoji-button@4.6.2/dist/emoji-button.umd.min.js"></script>

    <style>
        /* Sayfa genel stil ayarları */
        body {
            margin: 0;
            padding: 0;
            background-color: #ece5dd;
            font-family: 'Segoe UI', sans-serif;
        }

        /* Dashboard'a dönüş butonu */
        .btn-back {
            background-color: #cccccc;
            color: #333;
            border: none;
            padding: 8px 16px;
            border-radius: 20px;
            font-size: 14px;
            cursor: pointer;
            font-weight: bold;
            transition: background-color 0.3s;
        }

        .btn-back:hover {
            background-color: #bbbbbb;
        }

        /* Ana sohbet kutusu */
        .chat-container {
            display: flex;
            width: 90%;
            height: 80vh;
            margin: 20px auto;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
            background-color: #ffffff;
        }

        /* Kullanıcı listesi stili */
        .user-list {
            width: 250px;
            background-color: #f0f0f0;
            padding: 10px;
            border-right: 1px solid #ddd;
            overflow-y: auto;
        }

        .user-card {
            display: flex;
            align-items: center;
            padding: 8px 10px;
            background-color: #f5f5f5;
            border-radius: 5px;
            margin-bottom: 8px;
            text-decoration: none;
            color: #333;
            transition: background-color 0.3s;
        }

        .user-card:hover {
            background-color: #e0e0e0;
        }

        .user-card .avatar {
            font-size: 20px;
            margin-right: 10px;
        }

        .user-card .username {
            font-weight: bold;
        }

        .active-user {
            background-color: #d0f0c0;
            border: 1px solid #25d366;
        }

        /* Sohbet alanı */
        .chat-area {
            flex: 1;
            display: flex;
            flex-direction: column;
            background-color: #e5ddd5;
        }

        .chat-header {
            padding: 10px;
            background-color: #075e54;
            color: white;
            font-weight: bold;
        }

        .messages {
            flex: 1;
            padding: 15px;
            overflow-y: auto;
        }

        .message-bubble {
            max-width: 60%;
            padding: 10px 14px;
            margin: 6px 0;
            border-radius: 8px;
            position: relative;
            font-size: 14px;
            clear: both;
            line-height: 1.4;
        }

        .sent {
            background-color: #dcf8c6;
            margin-left: auto;
            border-bottom-right-radius: 0;
        }

        .received {
            background-color: #ffffff;
            margin-right: auto;
            border-bottom-left-radius: 0;
        }

        .timestamp {
            display: block;
            font-size: 11px;
            color: #999;
            margin-top: 4px;
            text-align: right;
        }

        /* Mesaj yazma alanı */
        .send-area {
            padding: 10px;
            display: flex;
            align-items: center;
            border-top: 1px solid #ccc;
            background-color: #f0f0f0;
        }

        .message-input {
            flex: 1;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 20px;
            font-size: 14px;
        }

        .send-button {
            background-color: #25d366;
            color: white;
            border: none;
            padding: 8px 16px;
            margin-left: 8px;
            border-radius: 50px;
            cursor: pointer;
            font-weight: bold;
            transition: background-color 0.3s;
        }

        .send-button:hover {
            background-color: #1ebc57;
        }

        .emoji-button {
            margin-left: 8px;
            border: none;
            background: none;
            font-size: 20px;
            cursor: pointer;
        }
    </style>

    <script type="text/javascript">
        var chat;

        $(function () {
            var userId = '<%= Session["UserID"] %>'; // Oturum açmış kullanıcı ID'si
            var receiverId = '<%= Request.QueryString["to"] %>'; // Mesaj gönderilecek kişi ID'si

            $.connection.hub.qs = { 'userid': userId }; // SignalR bağlantısına kullanıcı ID’sini ekle
            chat = $.connection.chatHub;

            // Sunucudan mesaj geldiğinde çalışacak fonksiyon
            chat.client.receiveMessage = function (senderId, receiverId, senderUsername, message) {
                var currentUserId = '<%= Session["UserID"] %>';
                if (receiverId === currentUserId) {
                    var time = new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

                    var msgHtml = '<div class="message-bubble received">' +
                        '<b>' + senderUsername + '</b><br />' +
                        message +
                        '<div class="timestamp">' + time + '</div>' +
                        '</div>';

                    $('#messages').append(msgHtml);
                    $('#messages').scrollTop($('#messages')[0].scrollHeight);
                }
            };

            // Yazıyor bildirimi
            chat.client.showTyping = function (username) {
                if ($("#typing-indicator").length === 0) {
                    $('#messages').append('<div id="typing-indicator"><i>' + username + ' yazıyor...</i></div>');
                    $('#messages').scrollTop($('#messages')[0].scrollHeight);
                }

                clearTimeout(window.typingTimeout);
                window.typingTimeout = setTimeout(() => {
                    $('#typing-indicator').remove();
                }, 3000);
            };

            // SignalR bağlantısı başarılıysa
            $.connection.hub.start().done(function () {
                // Gönder butonuna tıklanınca mesajı gönder
                $('#btnSend').click(function () {
                    var msg = $('#txtMessage').val();
                    if (msg.trim() !== '') {
                        var time = new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });

                        var msgHtml = '<div class="message-bubble sent">' +
                            '<b>Ben</b><br />' +
                            msg +
                            '<div class="timestamp">' + time + ' <span style="color:gray">(Gönderildi)</span></div>' +
                            '</div>';

                        $('#messages').append(msgHtml);
                        $('#messages').scrollTop($('#messages')[0].scrollHeight);

                        chat.server.send(userId, receiverId, msg); // SignalR ile sunucuya gönder
                        $('#txtMessage').val('');
                    }
                });

                // Emoji butonu açma
                const picker = new EmojiButton({ position: 'top-end', zIndex: 9999 });
                const btnEmoji = document.getElementById("btnEmoji");

                btnEmoji.addEventListener('click', () => {
                    picker.togglePicker(btnEmoji);
                });

                picker.on('emoji', emoji => {
                    const msgInput = document.getElementById('txtMessage');
                    msgInput.value += emoji;
                    msgInput.focus();
                });

                // Yazma bildirimi
                $('#txtMessage').on('input', function () {
                    chat.server.typing(userId, receiverId);
                });
            }).fail(function (error) {
                console.log("SignalR bağlantı hatası:", error);
            });
        });
         </script>
         </head>

         <body>
         <form id="form1" runat="server">
         <!-- Dashboard’a Geri Dön Butonu -->
         <div style="width:90%; margin: 20px auto 0;">
            <asp:Button ID="btnBackToDashboard" runat="server" Text="⬅" CssClass="btn-back" OnClick="btnBackToDashboard_Click" />
         </div>

         <div class="chat-container">
            <!-- Kullanıcı Listesi -->
            <div class="user-list">
                <h4>Kullanıcılar</h4>
                <asp:Repeater ID="rptUsers" runat="server">
                    <ItemTemplate>
                        <!-- Kullanıcı kartı bağlantısı -->
                        <a href='Chat.aspx?to=<%# Eval("UserID") %>' class='user-card <%# Eval("UserID").ToString() == Request.QueryString["to"] ? "active-user" : "" %>'>
                            <div class="avatar">👤</div>
                            <div class="username"><%# Eval("Username") %></div>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- Sohbet Alanı -->
            <div class="chat-area">
                <div class="chat-header">
                    Sohbet
                </div>

                <!-- Mesajların listelendiği alan -->
                <div class="messages" id="messages">
                    <asp:Literal ID="literalMessages" runat="server" />
                </div>

                <!-- Mesaj gönderme alanı -->
                <div class="send-area">
                    <input type="text" id="txtMessage" class="message-input" placeholder="Mesaj yaz..." />
                    <button id="btnEmoji" type="button" class="emoji-button">😊</button>
                    <input type="button" id="btnSend" value="➤" class="send-button" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
