namespace TTMusteriTemsilcisiAPI.Models
{
    /// <summary>
    /// Author: Yağız AKYÜZ
    /// Date: 28.09.2025
    /// Email: akyzyagiz@gmail.com
    /// Description: Bu class abone eklemek için, abonelerin şahsi ve adres bilgileri alınacak
    /// ve ekleme yapan müşteri temsilcisinin id'si eklenerek veritabanına yollanacak.
    /// En sonda telkotürk çalışanları abonelik işlemini bitirdiğinde ProcessFinished kısmını true yaparak
    /// işlemi sonlandıracaklar.
    /// AddedDate ile sisteme eklendiği tarihi tutuyoruz.
    /// </summary>
    public class SubscriberClass
    {
        public int Id { get; set; }
        public int CustomerRepresentativeID { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public long? TCKN { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public DateTime AddedDate { get; set; }
        public bool ProcessFinished { get; set; }
    }
}
