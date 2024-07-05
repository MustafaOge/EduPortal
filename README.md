
# Elektrik Dağıtım Uygulaması Portalı

Edu Portal, iş yeri eğitimi sürecinde geliştirdiğim bir portal projesidir. Bu proje, elektrik dağıtım şirketi vezne kullanıcılarının müşterilere hizmet verirken kullandığı senaryoları simüle eden bir yazılım olarak geliştirildi. 

## Kurulum ve Çalıştırma

1. **Depoyu klonlayın:**
    ```bash
    git clone https://github.com/MustafaOge/EduPortal.git
    cd EduPortal
    ```

2. **Bağımlılıkları yükleyin:**
    ```bash
    dotnet restore
    ```

3. **Yapılandırma ayarlarını güncelleyin:**
    - `appsettings.json` dosyasındaki veritabanı bağlantı dizelerini yapılandırın.
    - RabbitMQ ve Redis yapılandırmalarını ayarlayın.

4. **Uygulamayı çalıştırın:**
    ```bash
    dotnet run
    ```
## Özellikler

### Abone Yönetimi
- Abone Kaydı: Yeni abone kaydı oluşturma.
- Abone Sonlandırma: Aboneliği sonlandırma işlemi ve sonlandırma bildirimi gönderme.
- Abonelik Sorgulama: Abonelik bilgilerini sorgulama.

### Fatura İşlemleri
- Fatura Ödeme: Abonelerin faturalarını ödeme.
- Fatura Sorgulama: Abonelerin fatura detaylarını sorgulama.
- Fatura İtirazı Oluşturma: Abonelerin fatura itirazlarını oluşturma.
- Son Ödeme Tarihi Yaklaşan Fatura Bildirimi Gönderme: Son ödeme tarihi yaklaşan fatura bildirimleri gönderme.

### Kesinti Yönetimi
- Elektrik Planlı Kesinti Bilgilerini Görüntüleme: Planlanmış elektrik kesintilerinin bilgilerini görüntüleme.
- Kesinti Gerçekleşen Mahallelerdeki Abonelere Kesinti Bildirimi Gönderme: Kesinti gerçekleşen mahallelerdeki abonelere kesinti bildirimleri gönderme.

### Destek Talepleri
- Müşteri Desteği ve Durum Takibi.
## Teknoloji




- ASP.NET 8 Core MVC
- MS SQL / CouchBase
- RabbitMQ
- Redis
- REST Services / Refit
- Graylog
- XUnit

## Mimari

Projede, .NET Core tabanlı clean architecture yaklaşımı benimsenmiştir. Aşağıda projenin ana bileşenleri ve bu bileşenlerin rolleri açıklanmıştır:

### Domain Katmanı
Domain katmanı, uygulamanın iş mantığını, iş kurallarını ve varlıklarını içeren temel bileşendir. Bu katman, uygulamanın çekirdeğini oluşturur ve iş süreçlerinin nasıl gerçekleştirileceğini belirler. Genellikle veritabanı bağımsızdır ve yalnızca işletme mantığını temsil eder.


### Application Katmanı
Application katmanı, iş mantığını uygulayan ve veri aktarımını yöneten bileşendir. Bu katman, servisler, veri transfer objeleri (DTOs) gibi bileşenlerin yanı sıra arka planda çalışan işlemleri yönetmek için Hangfire ve Redis gibi bileşenleri de içerebilir.

#### Servisler
- **Servisler:** İş mantığını içeren sınıflardır. `SubscriberService` gibi servisler, belirli işlevler için gerekli iş mantığını ve iş akışını yönetir. Servisler, repository'leri ve diğer gerekli bileşenleri kullanarak iş mantığını uygular.

#### Arka Plan İşleri ve Önbellek İşlemleri
- **Hangfire:** Arka planda çalışan işlemleri planlamak, yönetmek ve izlemek için kullanılır. Örneğin, ödenmemiş fatura bildirimleri gibi zaman alıcı ve tekrarlanan işlemleri kolayca yönetebilir.

- **Redis:** Özellikle abonelik bilgileri gibi hızlı ve geçici veri saklama gereksinimlerinde kullanılır.

- **RabbitMQ:** Asenkron işlemleri sıralamak ve yönetmek için kullanılır, örneğin abonelik sonlandırma email'leri göndermek gibi.







### Persistence Katmanı
Persistence katmanı, veritabanı işlemlerini yöneten bileşendir. Bu katman, Repository ve Unit of Work desenlerini içerir.

#### Repository Katmanı
- **Generic Repository:** CRUD (Create, Read, Update, Delete) işlemleri için genel bir temel oluşturur. `GenericRepository<TEntity, TKey>` sınıfı, tüm varlıklar için ortak olan operasyonları içerir.
- **Uzmanlaşmış Repository'ler:** Belirli varlık türleri için özelleştirilmiş işlemleri gerçekleştirir. Örneğin, `AccountRepository` sınıfı, hesaplarla ilgili özelleştirilmiş sorgular ve işlemleri içerir.

#### Unit of Work
- **UnitOfWork:** Veritabanı işlemlerinin atomik olarak yönetilmesini sağlar. `UnitOfWork` sınıfı, veritabanı işlemlerinin başlatılması, commit edilmesi ve geri alınması işlevlerini yönetir.

### MVC Katmanı
MVC (Model-View-Controller) katmanı, kullanıcı arayüzünü ve HTTP isteklerini işleyen ana bileşendir. Bu katman, uygulamanın kullanıcıyla etkileşimde bulunduğu noktadır ve şu ana bileşenleri içerir:

- **Controller'lar:** HTTP isteklerini karşılayan ve uygun iş mantığı servislerini çağıran sınıflardır. Her bir controller, belirli bir işlevselliği temsil eder ve gelen isteklere göre ilgili işlemleri yönlendirir.

- **Views (Görünümler):** Kullanıcı arayüzünün sunulduğu bileşenlerdir. HTML, CSS ve JavaScript gibi web teknolojileri kullanılarak oluşturulurlar. Controller'lar tarafından oluşturulan veri ve iş mantığı sonuçları, views aracılığıyla kullanıcıya gösterilir.

- **Models:** Uygulamanın iş mantığını ve veri yapısını temsil eden sınıflardır. Genellikle Domain katmanından gelen verileri ve iş mantığını views ile iletişim haline getirirler. Models, views'in ihtiyaç duyduğu veriye erişim sağlar ve kullanıcıdan gelen verileri işlemek için kullanılır.

MVC katmanı, uygulamanın modüler yapısını ve kullanıcı arayüzü ile iş mantığı arasındaki bağlantıyı sağlar. Bu sayede, uygulama hem kullanıcı dostu bir arayüze sahip olur hem de işlevselliği açısından esneklik kazanır.


### Veri Aktarım Objeleri (DTOs)
- **DTOs:** Servis katmanı ile Persistence katmanı arasında veri taşıyan objelerdir. Örneğin, `CreateAccountRequestDto` sınıfı, bir hesap oluşturmak için gerekli verileri taşır.

### AutoMapper
- **AutoMapper:** Nesneler arasındaki veri transferini kolaylaştırmak için kullanılır. Bu, özellikle veritabanı varlıkları ve DTO'lar arasında veri dönüşümü gerektiğinde yararlıdır.

### Validasyon
- **FluentValidation:** Gelen verilerin doğruluğunu ve tutarlılığını kontrol etmek için kullanılır. Bu, API katmanında veri validasyonu için etkin bir yol sağlar ve iş mantığı katmanına geçmeden önce girdilerin uygunluğunu garanti eder.

### Ek Bileşenler
- **EduPortal Info Worker Service:** Bu servis, API'den veri çekip veritabanına kaydeder ve ardından ana projeye verileri kuyruk aracılığıyla gönderir. Bu, mikroservis mimarisi ile çalışır.

Bu mimari, projenin genişleyebilir, sürdürülebilir ve test edilebilir olmasını sağlar. Ayrıca, farklı katmanların ayrılması, projenin daha kolay yönetilmesine ve geliştirilmesine olanak tanır.

## Testler

- Proje için XUnit testlerini çalıştırmak için `dotnet test` komutunu kullanın.
- Tüm testlerin başarıyla geçtiğinden emin olun.
 

## Veritabanı Diyagramı

![Logo](https://i.hizliresim.com/emzss85.png)

