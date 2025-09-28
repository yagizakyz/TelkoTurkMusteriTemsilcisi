using Microsoft.EntityFrameworkCore;

namespace TTMusteriTemsilcisiAPI.Models
{
    public class TTContext : DbContext
    {
        public TTContext(DbContextOptions<TTContext> options) : base(options)
        {

        }
        public DbSet<CustomerRepresentativeClass> CustomerRepresentativeTable { get; set; }
        public DbSet<SubscriberClass> SubscriberTable { get; set; }
        public DbSet<TTPanelClass> TTPanelTable { get; set; }
    }
}
