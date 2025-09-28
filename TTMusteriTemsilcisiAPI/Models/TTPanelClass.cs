namespace TTMusteriTemsilcisiAPI.Models
{
    /// <summary>
    /// Author: Yağız AKYÜZ
    /// Date: 28.09.2025
    /// Email: akyzyagiz@gmail.com
    /// Description: Bu class TTPanel uygulamasında kullanılacak müşteri temsilcisi bilgilerinin tutulduğu sınıftır.
    /// </summary>
    public class TTPanelClass
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
