İnternete yükleme işlemi: 

1. Adım: Bu kodları bilgisayarınıza indirin, ardından bir MSSQL server'ı olan servis alın ve burada orada TTMusteriTemsilciDB adında alan açın ardından burada bulunan TTMusteriTemsilciDB.sql dosyasının içini kopyalayın servisin içinde ki veritabanı query'i alanına yapıştırın böylelikle tablolar oluşacak.

2. Adım: Ardından API dosyası içinde bulunan appsettings.json dosyasına gidin ve orada ki DefaultConnection alanını serverda bulunan veritabanı yolu ile değiştirin.

3. Adım: Bunları yapmadan önce bir alan adı alın örneğin www.telkoturkmtapi.com ardından servisi alırken bu alan adını servise bağlayın. Dosyalarımı bu servise yüklüyoruz ve projemiz yayına giriyor. Linki: www.telkoturkmtapi.com/api/... olacak.

4. Adım: Örneğin müşteri temsilcisine giriş yaptırmak istiyorsunuz o zaman bağlantı olarak, www.telkoturkmtapi.com/api/CustomerRepresentative/LoginUser veya abone numarasına göre abone mi görmek istiyorsunuz o zaman www.telkoturkmtapi.com/api/Subscriber/GetSubscribersByPhoneNumber/[PhoneNumber] şeklinde olacak.


   

TelkoTürk Teknoloji Müşteri Temsilcisi  


Yağız AKYÜZ 

28.09.2025 

 

Veritabanı: Veritabanı olarak MSSQL kullandım, üç adet tablomuz var bunlar CustomerRepresentativeTable (Müşteri Temsilcisi Tablosu), SubscriberTable (Abone Tablosu) ve TTPanelTable (TelkoTürk çalışanlarının tablosu). 

- CustomerRepresentativeTable: Bu tabloda müşteri temsilcisinin ad, soyad, TCKN, telefon numarası, email adresi, şifre, kaydolma tarihi ve kaydını sildirme tarihini tutuyoruz. OTP sistemi maliyetli olacağını düşündüğüm için şifreli giriş yapmak daha mantıklı geldi. Çünkü OTP sistemi normal müşteriler için iyi olabilir, çok fazla şifre olduğu için insanlar karıştırıyor olabilir ancak müşteri temsilcileri buradan para kazanacağı için hem şifrelerini unutmazlar hemde biz SMS servisine ekstra masraf yapmamış oluruz. Bunun dışında OTP sistemi böyle maddi kazanç elde edilecek yerlerde pek sağlıklı olmayabilir. Şifrelemede JWT kullandım. 

- SubscriberTable: Bu tabloda abonenin kişisel bilgileri, ev adresi ve onu kaydeden temsilcinin id’sini tutuyoruz. Bunlar dışında sisteme eklendiği tarih ve işlemi bitti mi diye kontrol eden ProcessFinished alanı var. ProcessFinished ilk başta false olarak sisteme düşüyor, sisteme düştükten sonra TelkoTürk çalışanları abone ile iletişime geçip her şeyi onayladığında sistem üzerinden ProcessFinished’ı true yapar.

 
-TTPanelTable: TelkoTürk çalışanları için oluşturulmuş bir tablo. Gelecek revizelere göre düzenlenecektir. 


Projeyi .Net 8 ile oluşturdum, şifreleme sistemi JWT. Proje içine bulunan metotları aşağıda sıralayacağım ve en altta bulunan GitHub linki ile projeye giderek kodlama üzerinde gerekli incelemeleri yapabilirsiniz. 

-CustomerRepresentativeController 

RegisterUser: Müşteri temsilcisinin kaydını yapar. 

LoginUser: Müşteri temsilcisinin eposta ve şifre kullanarak girişini sağlar. 

GetAllCR: Bütün müşteri temsilcilerini listeler. 

GetCRByPhoneNumber: Telefon numarasına göre müşteri temsilcisini getirir. 

-SubscriberController 

Post: Abonelik ekler, ProcessFinished otomatik olarak false olarak eklenir. 

ProcessFinishedTrue: TelkoTürk çalışanları abonelik isteyen kişi ile iletişime geçip her şeyi onaylaması durumda bu metot ile ProcessFinished alanını true olarak işaretleyebilecek. 

GetAllSubscribers: Bütün aboneleri listeler. 

GetCRSubscribers: Müşteri temsilcisi id’sine göre aboneleri listeler. 

GetSubscribersByPhoneNumber: Abonenin telefon numarasına göre abonelik bilgilerini getirir. 

GetAllPF_False: ProcessFinished alanı false olan abonelikleri eklenme tarihine göre sıralar. 

GetAllPF_True: ProcessFinished alanı trueolan abonelikleri eklenme tarihine göre sıralar. 

 

-TTPanelController 

RegisterUser: Kullanıcı ekler. 

LoginUser: Eposta ve şifre ile kullanıcı giriş yapar. Yapılacak revizelere göre bu alan geliştirilecek. 
