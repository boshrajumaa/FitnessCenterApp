# 🏋️‍♂️ SAU Fitness Center - Yönetim ve Randevu Sistemi

> **Sakarya Üniversitesi - Web Programlama Dersi Projesi** > Modern, Yapay Zeka (AI) destekli ve REST API entegrasyonlu kapsamlı spor salonu yönetim platformu.

![Project Status](https://img.shields.io/badge/Status-Completed-success)
![Framework](https://img.shields.io/badge/.NET%20Core-7.0-purple)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-blue)

## 📖 Proje Hakkında
Bu proje, klasik spor salonu yönetim süreçlerini dijitalleştirmek amacıyla geliştirilmiştir. **ASP.NET Core MVC** mimarisi üzerine inşa edilen sistem, üyelerin eğitmenlerden randevu almasını sağlarken, arka planda karmaşık iş kurallarını (çakışma kontrolü, mesai saatleri) otomatik olarak yönetir. Ayrıca entegre **Yapay Zeka (AI)** modülü ile üyelere kişisel program önerileri sunar.

## ✨ Temel Özellikler (Features)

### 1. 🔐 Kimlik ve Yetkilendirme (Identity)
* **Rol Bazlı Erişim:** Admin ve Member (Üye) olmak üzere iki farklı yetki seviyesi.
* **Güvenli Giriş:** Şifreli üyelik sistemi, "Beni Hatırla" özelliği.
* **Admin Paneli:** Sadece yöneticilerin erişebildiği eğitmen ve hizmet yönetim ekranları.

### 2. 📅 Akıllı Randevu Sistemi (Business Logic)
* **Mesai Kontrolü:** Eğitmenlerin çalışma saatleri dışında (Örn: 09:00 - 18:00 harici) randevu alınamaz.
* **Çakışma Önleyici (Conflict Check):** Aynı eğitmene, aynı saatte ikinci bir randevu alınması sistem tarafından engellenir.
* **Validasyon:** Geçmiş tarihe randevu alınamaz, form verileri sunucu tarafında doğrulanır.

### 3. 🤖 Yapay Zeka (AI) Antrenör
* Kullanıcıların fiziksel özelliklerine (boy, kilo, hedef) göre simüle edilmiş AI algoritması çalışır.
* Kişiye özel antrenman ve beslenme programı çıktısı üretir.

### 4. 🌐 REST API ve Entegrasyon
* **JSON Veri Servisi:** Mobil uygulamalar ve 3. parti yazılımlar için dışa açılmış API.
* **Endpoint:** `/api/fitness/trainers` (Tüm eğitmen listesi).
* **LINQ ile Filtreleme:** `/api/fitness/search?skill=Yoga` (Uzmanlığa göre dinamik arama).

### 5. 🎨 Modern Arayüz (UI/UX)
* **Responsive Tasarım:** Mobil uyumlu, "Bootswatch Pulse" teması.
* **Glassmorphism:** Modern cam efektli istatistik kartları ve şeffaf menüler.
* **Etkileşim:** Hover efektleri, animasyonlu geçişler (Fade-in/Slide-up).

## 🛠 Kullanılan Teknolojiler

| Kategori | Teknoloji |
|----------|-----------|
| **Backend** | ASP.NET Core 7.0 MVC, C# |
| **Veritabanı** | MS SQL Server (LocalDB), Entity Framework Core (Code-First) |
| **Frontend** | HTML5, CSS3, JavaScript, Bootstrap 5 |
| **Güvenlik** | ASP.NET Core Identity |
| **API** | RESTful API, LINQ |
| **Sürüm Kontrol** | Git & GitHub |

## 🚀 Kurulum ve Çalıştırma (Installation)

Projeyi yerel makinenizde çalıştırmak için adımları izleyin:

1.  **Projeyi Klonlayın:**
    ```bash
    git clone [https://github.com/KULLANICI_ADINIZ/PROJE_ADINIZ.git](https://github.com/KULLANICI_ADINIZ/PROJE_ADINIZ.git)
    ```

2.  **Veritabanını Oluşturun:**
    Visual Studio'da **Package Manager Console**'u açın ve şu komutu çalıştırın:
    ```powershell
    Update-Database
    ```
    *(Bu işlem, LocalDB üzerinde veritabanını ve tabloları otomatik oluşturacaktır)*.

3.  **Projeyi Başlatın:**
    `F5` tuşuna basın veya "Run" butonuna tıklayın.

4.  **Giriş Bilgileri (Örnek):**
    * Sisteme kayıt olarak yeni bir üye oluşturabilir veya veritabanındaki admin hesabını kullanabilirsiniz.

## 📡 API Kullanımı

Proje çalışırken tarayıcı veya Postman üzerinden test edebilirsiniz:

* **Tüm Eğitmenleri Listele:**
  `GET https://localhost:PORT/api/fitness/trainers`
  
* **Uzmanlığa Göre Ara (Örn: Yoga):**
  `GET https://localhost:PORT/api/fitness/search?skill=Yoga`

* **Sistem İstatistikleri:**
  `GET https://localhost:PORT/api/fitness/stats`

## 🔜 Gelecek Planları (Future Roadmap)
* [ ] Mobil uygulama (Flutter) entegrasyonu.
* [ ] Online ödeme sistemi (Iyzico/Stripe) altyapısı.
* [ ] QR Kod ile salona giriş sistemi.

---

**öğrenci:** Büşra cuma
**Öğrenci No:** b221210552 
**Ders:** Web Programlama  
&copy; 2025 SAU Fitness Center. Tüm Hakları Saklıdır.