using CarRentalNovility.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalNovility.DataLayer
{
    public class CarRentalDbContext : DbContext
    {
        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options)
            : base(options)
        {
        }

        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
