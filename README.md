# ChatApplication
# 🩺 Gerçek Zamanlı Doktor-Hasta Sohbet Uygulaması

Bu proje, **ASP.NET WebForms**, **SignalR** ve **Access veritabanı** kullanılarak geliştirilmiş bir **doktor-hasta sohbet sistemidir**. Sistem, kullanıcı girişi sonrası doktor ve hastaların gerçek zamanlı olarak sadece karşı roldeki kullanıcılarla mesajlaşmasına olanak tanır.

## 🚀 Özellikler

- ✅ Gerçek zamanlı mesajlaşma (SignalR ile)
- ✅ Kullanıcı girişi ve rol kontrolü (doktor / hasta)
- ✅ Sadece doktor-hasta eşleşmelerine izin verme
- ✅ Gönderildi / Okundu mesaj durumu
- ✅ Emoji desteği 😊
- ✅ Yazıyor... göstergesi (typing indicator)
- ✅ Dashboard tasarımı (doktor ve hasta için ayrı)

## 🛠️ Kullanılan Teknolojiler

- **ASP.NET WebForms**
- **SignalR (v2.4.2)**
- **Microsoft Access (.accdb)**
- **jQuery**
- **Emoji Button (emoji-picker)**

## 🔐 Kullanıcı Rolleri

| Rol    | Açıklama                          |
|--------|-----------------------------------|
| Doctor | Hastalarla sohbet edebilir        |
| Patient| Doktorlarla sohbet edebilir       |

## 📁 Veritabanı Yapısı

### USERS

| Alan Adı   | Açıklama     |
|------------|--------------|
| UserID     | Otomatik ID  |
| Username   | Giriş adı    |
| Password   | Giriş şifresi|
| FullName   | Ad Soyad     |
| Role       | Doctor / Patient |

### CHAT

| Alan Adı    | Açıklama              |
|-------------|-----------------------|
| ChatID      | Otomatik ID           |
| SenderID    | Gönderen kullanıcı    |
| ReceiverID  | Alıcı kullanıcı       |
| MessageText | Mesaj içeriği         |
| Timestamp   | Gönderim zamanı       |
| IsRead      | Okundu bilgisi (bool) |




## 🔧 Kurulum

1. Bu projeyi Visual Studio ile açın.
2. `chat_db.accdb` veritabanını `App_Data` klasörüne yerleştirin.
3. SignalR ve jQuery kütüphanelerinin yüklü olduğundan emin olun.
4. Gerekirse `web.config` bağlantı cümlesini kontrol edin.
5. `Login.aspx` üzerinden kullanıcı girişi yapın.
## 
   
  Sisteme kayıtlı doktor kullanıcı adı ve şifresi: emin - 1, 
  Sisteme kayıtlı hasta kullanıcı adı ve şifresi: eren - 1


## Geliştiren

- **Muhammet Emin Türk**
- 📧 Email: meturk00@gmail.com

## ✨ Uygulama Görselleri

![Image](https://github.com/user-attachments/assets/d03ebb41-8a88-4138-8bc2-d65256128104)
![Image](https://github.com/user-attachments/assets/888ea887-7a45-4694-9223-4f20ced742cb)
![Image](https://github.com/user-attachments/assets/6f483898-a278-4847-9469-b6f07846f8c7)
![Image](https://github.com/user-attachments/assets/d8e11ae4-2b26-4882-a1ce-5c747447c754)
![Image](https://github.com/user-attachments/assets/6c90fec0-8864-4d44-99db-d65cbbb3d25e)
![Image](https://github.com/user-attachments/assets/dd4f7e4d-3988-4973-ba42-31e887a3b53b)
