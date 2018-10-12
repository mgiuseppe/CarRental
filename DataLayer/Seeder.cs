using CarRentalNovility.Entities;
using DataLayer.Repositories.Contracts;
using System;
using System.Threading.Tasks;

namespace CarRentalNovility.DataLayer
{
    [Obsolete("Now I use EFCore HasData methods to seed. I keep this class for future reference.")]
    public class Seeder
    {
        private readonly CarRentalDbContext ctx;
        private readonly IRepositoryClient repClient;
        private readonly IRepositoryCar repCar;
        private readonly IRepositoryReservation repReservation;
        private readonly IUnitOfWork uow;

        public Seeder(CarRentalDbContext ctx, IRepositoryClient repClient, IRepositoryCar repCar, IRepositoryReservation repReservation, IUnitOfWork uow)
        {
            this.ctx = ctx;
            this.repClient = repClient;
            this.repCar = repCar;
            this.repReservation = repReservation;
            this.uow = uow;
        }

        public async Task Seed()
        {
            ctx.Database.EnsureCreated();
            AddClients();
            AddCarsAndCarTypes();
            await uow.CompleteAsync();
            await AddReservations();
            await uow.CompleteAsync();
        }

        private void AddClients()
        {
            repClient.Insert(new Client() { FullName = "Test 1", Email ="test1@test.com", PhoneNumber="0804590101"});
            repClient.Insert(new Client() { FullName = "Test 2", Email = "test2@test.com", PhoneNumber = "0804590102" });
            repClient.Insert(new Client() { FullName = "Test 3", Email = "test3@test.com", PhoneNumber = "0804590103" });
        }

        private void AddCarsAndCarTypes()
        {
            repCar.Insert(new Car() { PlateNumber = "AA01BB", Type = new CarType() { Name = "standard", RentalRateFee = 10, CancellationRateFee = 5, DepositFee = 0.5m } });
            repCar.Insert(new Car() { PlateNumber = "BB01CC", Type = new CarType() { Name = "family", RentalRateFee = 100, CancellationRateFee = 50, DepositFee = 0.9m } });;
        }

        private async Task AddReservations()
        {
            var car1 = await repCar.GetAsync(1);
            var car2 = await repCar.GetAsync(2);
            var client = await repClient.GetAsync(1);
            repReservation.Insert(new Reservation()
            {
                Car = car1,
                Client = client,
                ClientAccount = new ClientAccount () {
                    RentalRateFeeValueAtBookingMoment = car1.Type.RentalRateFee,
                    CancellationFeeValueAtBookingMoment = car1.Type.CancellationRateFee,
                    DepositFeePaid = 10
                },
                PickUpDateTime = DateTime.Now,
                ReturnDateTime = DateTime.Now.AddHours(2),
            });
            repReservation.Insert(new Reservation()
            {
                Car = car2,
                Client = client,
                ClientAccount = new ClientAccount()
                {
                    RentalRateFeeValueAtBookingMoment = car1.Type.RentalRateFee,
                    CancellationFeeValueAtBookingMoment = car1.Type.CancellationRateFee,
                    DepositFeePaid = 20
                },
                PickUpDateTime = DateTime.Now.AddHours(2),
                ReturnDateTime = DateTime.Now.AddHours(12),
            });
        }

    }
}
