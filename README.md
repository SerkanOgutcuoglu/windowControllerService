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
