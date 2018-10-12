using CarRentalNovility.Entities;
using Microsoft.EntityFrameworkCore;
using System;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var carType1 = new CarType { Id = 1, Name = "Family Car", RentalRateFee = 10, CancellationRateFee = 5, DepositFee = 0.2m };
            var carType2 = new CarType { Id = 2, Name = "Sport Car", RentalRateFee = 100, CancellationRateFee = 50, DepositFee = 0.2m };
            var client1 = new Client { Id = 1, FullName = "Giuseppe M", Email = "giuseppem@test.com", PhoneNumber = "3476010101" };
            var client2 = new Client { Id = 2, FullName = "Barba N", Email = "barban@test.com", PhoneNumber = "3476010202" };
            var clientAccount = new ClientAccount { Id = 1, RentalRateFeeValueAtBookingMoment = carType1.RentalRateFee, CancellationFeeValueAtBookingMoment = carType1.CancellationRateFee, DepositFeePaid = carType1.RentalRateFee * carType1.DepositFee };
            var car1 = new { Id = (long)1, PlateNumber = "AA00BB", TypeId = 1 };
            var car2 = new { Id = (long)2, PlateNumber = "AA01BB", TypeId = 1 };
            var car3 = new { Id = (long)3, PlateNumber = "AA00SP", TypeId = 2 };
            var reservation1 = new
            {
                Id = (long)1,
                PickUpDateTime = DateTime.Now,
                ReturnDateTime = DateTime.Now.AddHours(8),
                State = ReservationState.Booked,
                CarId = (long)1,
                ClientId = (long)1,
                ClientAccountId = (long)1
            };

            modelBuilder.Entity<CarType>().HasData(carType1, carType2);
            modelBuilder.Entity<Client>().HasData(client1, client2);
            modelBuilder.Entity<Car>().HasData(car1, car2, car3);
            modelBuilder.Entity<ClientAccount>().HasData(clientAccount);
            modelBuilder.Entity<Reservation>().HasData(reservation1);
        }
    }
}
