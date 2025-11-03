# CourseApp - Kurs Yönetim Sistemi

## 📋 Proje Hakkında

CourseApp, eğitim kurumları için geliştirilmiş kapsamlı bir kurs yönetim sistemidir. Bu uygulama ile öğrenci kayıtları, kurs yönetimi, ders programları, sınavlar ve sonuçları gibi tüm eğitim süreçleri kolayca yönetilebilir.

## 🏗️ Proje Mimarisi

Bu proje **Katmanlı Mimari (Layered Architecture)** prensiplerine uygun olarak geliştirilmiştir.

### Katmanlar

```
CourseApp/
├── 📁 EntityLayer/                 # Varlık Katmanı
│   ├── Entity/                     # Domain nesneleri
│   └── Dto/                        # Veri transfer nesneleri
├── 📁 CourseApp.DataAccessLayer/   # Veri Erişim Katmanı
│   ├── Abstract/                   # Repository arayüzleri
│   ├── Concrete/                   # Repository implementasyonları
│   ├── Migrations/                 # EF Core migration dosyaları
│   └── UnitOfWork/                 # Unit of Work pattern
├── 📁 CourseApp.ServiceLayer/      # İş Mantığı Katmanı
│   ├── Abstract/                   # Servis arayüzleri
│   ├── Concrete/                   # Servis implementasyonları
│   ├── Mapping/                    # AutoMapper profilleri
│   └── Utilities/                  # Yardımcı sınıflar
└── 📁 CourseApp.DesktopUI/         # Sunum Katmanı
    ├── Forms/                      # Windows Forms
    └── Utilities/                  # UI yardımcı sınıflar
```

## 🛠️ Kullanılan Teknolojiler

- **Framework:** .NET 8.0
- **UI:** Windows Forms
- **ORM:** Entity Framework Core
- **Veritabanı:** SQL Server / LocalDB
- **Design Patterns:** 
  - Repository Pattern
  - Unit of Work Pattern
  - Dependency Injection
  - SOLID Principles

## 📊 Veri Modeli

### Ana Varlıklar

- **Student (Öğrenci)**: Öğrenci bilgileri (Ad, Soyad, TC, Doğum Tarihi)
- **Course (Kurs)**: Kurs bilgileri (Kurs Adı, Başlangıç/Bitiş Tarihi, Fiyat)
- **Instructor (Eğitmen)**: Eğitmen bilgileri
- **Lesson (Ders)**: Ders bilgileri ve programları
- **Registration (Kayıt)**: Öğrenci-Kurs kayıt ilişkileri
- **Exam (Sınav)**: Sınav bilgileri
- **ExamResult (Sınav Sonucu)**: Öğrenci sınav sonuçları

## 🚀 Kurulum

### Gereksinimler

- Visual Studio 2022 veya VS Code
- .NET 8.0 SDK
- SQL Server veya SQL Server LocalDB

### Adımlar

1. **Projeyi klonlayın:**
   ```bash
   git clone [repository-url]
   cd CourseApp
   ```

2. **NuGet paketlerini geri yükleyin:**
   ```bash
   dotnet restore
   ```

3. **Veritabanı bağlantı dizesini yapılandırın:**
   - `CourseApp.DataAccessLayer/Concrete/AppDbContext.cs` dosyasında connection string'i düzenleyin

4. **Veritabanı migration'larını çalıştırın:**
   ```bash
   dotnet ef database update --project CourseApp.DataAccessLayer
   ```

5. **Projeyi çalıştırın:**
   ```bash
   dotnet run --project CourseApp.DesktopUI
   ```

## 💻 Kullanım

### Ana Özellikler

1. **Öğrenci Yönetimi**
   - Öğrenci kayıt, güncelleme, silme
   - Öğrenci listesi görüntüleme
   - Öğrenci detay bilgileri

2. **Kurs Yönetimi**
   - Kurs oluşturma ve düzenleme
   - Kurs programı belirleme
   - Eğitmen atama

3. **Kayıt İşlemleri**
   - Öğrenci-kurs eşleştirme
   - Kayıt durumu takibi
   - Kayıt geçmişi

4. **Sınav Yönetimi**
   - Sınav oluşturma
   - Sınav sonuçları girişi
   - Rapor alma

5. **Ders Programı**
   - Ders planlaması
   - Zaman çizelgesi oluşturma

## 🏛️ Mimari Desenler

### Repository Pattern
```csharp
public interface IGenericRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);
}
```

### Unit of Work Pattern
```csharp
public interface IUnitOfWork
{
    IStudentRepository Students { get; }
    ICourseRepository Courses { get; }
    IInstructorRepository Instructors { get; }
    // ... diğer repository'ler
    Task<int> SaveChangesAsync();
}
```

### Dependency Injection
Tüm bağımlılıklar `Program.cs` dosyasında yapılandırılmıştır.

## 📁 Proje Yapısı Detayı

### EntityLayer
- **BaseEntity**: Tüm varlıklar için temel sınıf
- **Entity klasörü**: Domain model sınıfları
- **Dto klasörü**: Veri transfer nesneleri

### DataAccessLayer
- **Abstract**: Repository arayüzleri
- **Concrete**: Repository implementasyonları ve DbContext
- **Migrations**: EF Core migration dosyaları
- **UnitOfWork**: Transaction yönetimi

### ServiceLayer (BusinessLayer)
- **Abstract**: İş mantığı arayüzleri
- **Concrete**: İş mantığı implementasyonları
- **Mapping**: AutoMapper yapılandırmaları
- **Utilities**: İş mantığı yardımcı sınıflar

### DesktopUI
- **Forms**: Windows Forms kullanıcı arayüzleri
- **Utilities**: UI yardımcı sınıflar

## 🔧 Geliştirme

### Yeni Özellik Ekleme

1. **Entity oluşturun** (EntityLayer/Entity/)
2. **Repository arayüzü tanımlayın** (DataAccessLayer/Abstract/)
3. **Repository implementasyonu yazın** (DataAccessLayer/Concrete/)
4. **Servis katmanını oluşturun** (ServiceLayer/)
5. **UI formunu geliştirin** (DesktopUI/Forms/)

### Kod Standartları

- SOLID prensiplerine uygun kod yazın
- Async/await pattern'ini kullanın
- Repository pattern'i takip edin
- Dependency Injection kullanın


## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -am 'Yeni özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/YeniOzellik`)
5. Pull Request oluşturun


## 🙏 Teşekkürler

## 📋 TODO Listesi

- [ ] Web API katmanı eklenmesi
- [ ] Authentication/Authorization sistemi
- [ ] Rapor modülü geliştirmesi
- [ ] Mail notification sistemi
- [ ] Mobile uygulama entegrasyonu

---
