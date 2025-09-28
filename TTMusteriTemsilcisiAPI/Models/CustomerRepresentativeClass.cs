namespace TTMusteriTemsilcisiAPI.Models
{
    /// <summary>
    /// Author: Yağız AKYÜZ
    /// Date: 28.09.2025
    /// Email: akyzyagiz@gmail.com
    /// Description: Bu class müşteri temsilcilerinin bizde olacak bilgilerinin sınıfıdır.
    /// Müşteri temsilcileri gerekli bilgileri verecekler ve giriş yaparken eposta ve şifre kullanmak zorundalar.
    /// Ayrıca temsilcilerin üye olduğu tarihi ve üyeliklerinin bittiği tarihide tutuyoruz.
    /// </summary>
    public class CustomerRepresentativeClass
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public long? TCKN { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? MembershipDate { get; set; }
        public DateTime? MembershipEndDate { get; set; }
    }
}
