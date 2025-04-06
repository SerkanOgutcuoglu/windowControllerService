# 🪄 WindowsServiceProject

Bu proje, **Windows servislerinin GUI (grafik arayüz) işlemlerini doğrudan gerçekleştirememesi** kısıtını aşmak amacıyla geliştirilmiştir. Normal şartlarda Windows servisleri sistemin arka planında çalışır ve güvenlik nedeniyle doğrudan kullanıcı oturumu ile etkileşime geçemezler. Örneğin; aktif pencerenin başlığını almak, pencereyi küçültmek veya bir butona tıklamak gibi işlemler servisler tarafından doğrudan yapılamaz.

Bu nedenle, bu projede aşağıdaki yaklaşım uygulanmıştır:

- Windows Servisi sadece arka planda çalışır ve GUI işlemleri yapmaz.
- Servis, kullanıcı oturumunda çalışacak ayrı bir **konsol uygulaması** başlatır.
- Konsol uygulaması, kullanıcının oturumunda çalıştığı için pencere ile etkileşime geçebilir (örneğin pencere başlığını alabilir).
- İşlemler tamamlandığında konsol uygulaması kendini kapatır.

---

## 📁 Proje Yapısı

```
WindowsServiceProject/
├── WindowControlService/        # Servis projesi
│   └── Controller.cs
│
├── windowController/            # Konsol uygulaması (GUI işlemlerini yapar)
│   └── Program.cs
│
└── README.md
```

---

## 🎯 Amaç

- Servisler GUI işlemleri gerçekleştiremediği için bu görev kullanıcı tarafında çalışan ayrı bir uygulamaya devredilir.
- Kullanıcının aktif oturumu tespit edilir.
- Konsol uygulaması bu oturumda başlatılır.
- Konsol uygulaması GUI işlemlerini yapar ve kapatılır.

---

## 🚀 Temel Özellikler

- Aktif pencerenin başlığı alınır.
- Bu başlık bir `.txt` dosyasına kaydedilir.
- Konsol uygulaması işlem bittiğinde kendini otomatik olarak kapatır.

---

## ⚙️ Gereksinimler

- .NET Framework 4.7.2 veya .NET 6+
- Visual Studio
- Windows 10 veya üzeri
- Yönetici (admin) yetkisi

---

## 📦 Kurulum

1. `windowController` projesini build edin ve çıktısını `WindowControlService` klasörüne kopyalayın.
2. `WindowControlService` projesini build edin.
3. Aşağıdaki komutları kullanarak servisi kurun ve başlatın:

```bash
sc create WindowControlService binPath= "C:\Path\To\WindowControlService.exe"
sc start WindowControlService
```

> Not: Yol bilgisini kendi sisteminize göre ayarlayın.

---

## 🧠 Teknik Açıklama

### 💡 Windows'ta Oturum (Session) Yapısı

Windows işletim sistemi, farklı kullanıcıların aynı anda oturum açmasına ve çalışmasına olanak tanır. Bu oturumlar, "Session" kavramıyla temsil edilir:

- **Session 0**: Sistem servislerinin çalıştığı oturumdur. Tüm servisler burada izole şekilde çalışır. Grafik arayüzle (GUI) etkileşim kuramazlar.
- **Session 1 ve üstü**: Kullanıcıların aktif olarak masaüstü ortamında çalıştığı oturumlardır. Kullanıcının başlattığı tüm uygulamalar (masaüstü, tarayıcı, vs.) bu oturumlarda çalışır.

2006'dan itibaren (Windows Vista ve sonrası), **Session 0 Isolation** adlı güvenlik özelliği sayesinde servisler kullanıcı arayüzüyle doğrudan iletişim kuramaz hâle gelmiştir. Bu güvenlik önlemi, kötü amaçlı servislerin kullanıcıyla etkileşimini engellemek için getirilmiştir.

### 🔄 Oturumlar Arası İletişim

Servisin kullanıcı oturumunda işlem başlatabilmesi için aşağıdaki adımlar izlenir:

1. `WTSGetActiveConsoleSessionId()` ile aktif kullanıcı oturumunun ID'si alınır.
2. Bu oturumun erişim yetkileri `WTSQueryUserToken` ile elde edilir.
3. Kullanıcı token’ı birincil token’a dönüştürülür (gerekirse `DuplicateTokenEx` kullanılır).
4. `CreateProcessAsUser()` fonksiyonu ile bu oturumda çalışan bir işlem başlatılır.

Bu işlem sayesinde servis, kullanıcı oturumunda bir uygulama başlatmış olur ve GUI işlemleri bu uygulama tarafından gerçekleştirilir.

- Windows servisleri session 0'da çalışır, oysa kullanıcı arayüzü session 1 veya üstünde çalışır.
- `WTSGetActiveConsoleSessionId` ile aktif oturum ID’si alınır.
- `CreateProcessAsUser` fonksiyonu kullanılarak bu oturumda işlem başlatılır.
- Konsol uygulaması `GetForegroundWindow` ve `GetWindowText` gibi API’lerle pencere bilgilerini alır.

🚀 Çalıştırma Talimatları
Kod içerisindeki path'leri kendi sisteminize göre ayarlayın.

Servisi eklemek için aşağıdaki komutları kullanın:

bash
sc create WindowControlService binPath= "C:\Path\To\WindowControlService.exe"
sc start WindowControlService


Servisi başlattığınızda, konsol uygulaması açılacak ve mevcut pencere küçülüp büyüyecektir, ardından kapanacaktır.

Bu basit uygulama, GUI işlemleri yapabilen servisler için bir temel oluşturur ve daha karmaşık algoritmalar geliştirebilmenize yardımcı olabilir.

**********----**********

# 🪄 WindowsServiceProject

This project is developed to overcome the limitation that **Windows services cannot directly perform GUI (graphical user interface) operations**. Normally, Windows services run in the background and, due to security restrictions, cannot interact directly with the user session. For example, operations like getting the title of the active window, minimizing the window, or clicking a button cannot be done directly by services.

Therefore, the following approach is applied in this project:

- The Windows Service runs only in the background and does not perform GUI operations.
- The service launches a separate **console application** that runs in the user session.
- Since the console application runs in the user's session, it can interact with the window (for example, it can get the window title).
- Once the operations are completed, the console application closes itself.

---

## 📁 Project Structure

WindowsServiceProject/ ├── WindowControlService/ # Service project │ └── Controller.cs │ ├── windowController/ # Console application (performs GUI operations) │ └── Program.cs │ └── README.md

yaml
Kopyala
Düzenle

---

## 🎯 Purpose

- Since services cannot perform GUI operations, this task is delegated to a separate application that runs on the user's side.
- The active user session is detected.
- A console application is started in this session.
- The console application performs GUI operations and then closes itself.

---

## 🚀 Key Features

- The title of the active window is retrieved.
- This title is saved in a `.txt` file.
- The console application automatically closes after completing the task.

---

## ⚙️ Requirements

- .NET Framework 4.7.2 or .NET 6+
- Visual Studio
- Windows 10 or higher
- Administrator (admin) privileges

---

## 📦 Installation

1. Build the `windowController` project and copy its output to the `WindowControlService` folder.
2. Build the `WindowControlService` project.
3. Install and start the service using the following commands:


sc create WindowControlService binPath= "C:\Path\To\WindowControlService.exe"
sc start WindowControlService
Note: Adjust the path to your system.

🧠 Technical Explanation
💡 Session Structure in Windows
The Windows operating system allows multiple users to log in and work simultaneously. These sessions are represented by the "Session" concept:

Session 0: The session where system services run. All services run here in isolation and cannot interact with the graphical user interface (GUI).

Session 1 and above: The sessions where users actively work in the desktop environment. All user-launched applications (desktop, browsers, etc.) run in these sessions.

Since 2006 (Windows Vista and later), the Session 0 Isolation security feature prevents services from interacting directly with the user interface. This security measure was introduced to prevent malicious services from interacting with users.

🔄 Inter-Session Communication
To enable the service to start a process in the user session, the following steps are followed:

The active user session ID is obtained using WTSGetActiveConsoleSessionId().

The access rights of this session are obtained with WTSQueryUserToken.

The user token is converted into the primary token (if needed, DuplicateTokenEx is used).

A process running in this session is started using the CreateProcessAsUser() function.

This process allows the service to start an application in the user session, and the GUI operations are performed by this application.

Windows services run in session 0, whereas the user interface runs in session 1 or above.

The active session ID is obtained using WTSGetActiveConsoleSessionId.

A process is started in this session using the CreateProcessAsUser function.

The console application uses APIs like GetForegroundWindow and GetWindowText to retrieve window information.

**---**

Adjust the paths in the code to match your system.
To add the service, use the following command:

bash
```
sc create WindowControlService binPath= "C:\Path\To\WindowControlService.exe"
sc start WindowControlService
```
When the service starts, the console application will open, the active window will minimize and maximize, and then the application will close.
This simple application serves as a foundation for creating services that can perform GUI operations and can help you build more complex algorithms in the future.
