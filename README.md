# 🪄 WindowsServiceProject

Bu proje, bir **Windows Servisi** aracılığıyla kullanıcı oturumunda çalışan ayrı bir konsol uygulaması başlatmayı amaçlar. Konsol uygulaması, aktif pencere üzerinde belirli GUI işlemleri gerçekleştirir ve işini tamamladıktan sonra otomatik olarak kapanır.

---

## 📌 Amaç

- Servisler doğrudan kullanıcı arayüzüyle etkileşemez.
- Bu proje, bu kısıtı aşmak için kullanıcı oturumunda ayrı bir uygulama başlatarak GUI işlemlerini dışarıdan yapmayı hedefler.

---

## 🧱 Proje Yapısı

WindowsServiceProject/ ├── WindowControlService/ # Windows servisi │ └── Controller.cs │ ├── windowController/ # Konsol uygulaması │ └── Program.cs │ └── README.md

yaml
Kopyala
Düzenle

---

## 🔧 Temel Özellikler

- Aktif kullanıcı oturumu tespit edilir.
- Kullanıcı oturumunda ayrı bir uygulama başlatılır.
- Konsol uygulaması belirlenen işlemleri gerçekleştirir:
  - Örn: aktif pencereyi kontrol etme, başlığını alma, dosyaya yazma vb.
- İşlem tamamlandığında konsol uygulaması otomatik olarak kapanır.

---

## 🧠 Teknik Notlar

- Servis ve kullanıcı oturumu arasında güvenli bir şekilde işlem başlatmak için Windows API’lerinden faydalanılır.
- Servisin, ayrıcalıklı modda çalışması gerekebilir (örneğin `LocalSystem` hesabı).
- Konsol uygulaması, GUI işlemleri gerçekleştirecek şekilde kullanıcı modunda çalışır.

---

## 📋 Gereksinimler

- .NET Framework veya .NET Core/6+
- Visual Studio (tavsiye edilen IDE)
- Windows 10 veya üstü
- Yükleme ve çalıştırma için yönetici (admin) izni

---

## ⚠️ Uyarılar

- Windows servisleri GUI işlemlerini doğrudan yapamaz. Bu sebeple aracı bir uygulama gerekir.
- Microsoft tarafından servislerin etkileşimli oturumla çalışması önerilmemektedir. Bu çözüm, özel amaçlı senaryolar içindir.

---

## 📝 Lisans

Bu proje MIT lisansı ile lisanslanmıştır. Dilediğiniz gibi kullanabilir ve geliştirebilirsiniz.

---

## 🙋 Katkı

Katkı sağlamak isterseniz, `issue` açabilir veya `pull request` gönderebilirsiniz. Her türlü öneri ve geri bildirim memnuniyetle karşılanır!
