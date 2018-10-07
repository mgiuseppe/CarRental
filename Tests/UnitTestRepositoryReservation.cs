using CarRentalNovility.DataLayer;
using CarRentalNovility.Entities;
using DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class UnitTestRepositoryReservation
    {

        [Theory]
        [MemberData(nameof(BrowseReservationAsync_TestCases))]
        public async Task BrowseReservationAsync_Behaviour(BrowseReservationsParameters parameters, long[] reservationsIdsExpected)
        {
            //Arrange
            var ctx = GetMockedDbContext();
            ctx.Reservations.AddRange(
                BuildReservation(1, new DateTime(2018, 01, 01, 10, 10, 10), ReservationState.PickedUp, 1, "Test 1", "test1@test.com", "0804590101"),
                BuildReservation(2, new DateTime(2019, 01, 01, 10, 10, 10), ReservationState.PickedUp, 2, "Test 1", "test1@test.com", "0804590101"),
                BuildReservation(3, new DateTime(2019, 01, 01, 10, 10, 10), ReservationState.Booked, 3, "Test 2", "test2@test.com", "0804590202")
            );
            await ctx.SaveChangesAsync();
            var rep = new RepositoryReservation(ctx);

            //Act
            var reservations = await rep.BrowseReservationAsync(parameters);
            var reservationsIds = reservations.Select(r => r.Id);

            //Assert
            Assert.Equal(reservationsIdsExpected, reservationsIds);
        }

        public static TheoryData<BrowseReservationsParameters, long[]> BrowseReservationAsync_TestCases
        {
            get
            {
                var data = new TheoryData<BrowseReservationsParameters, long[]>();
                //Research by fullname
                data.Add(BuildParameters("Test 1", null, null, null, null, null),
                         new long[] { 1, 2 });
                //Research by email
                data.Add(BuildParameters(null, "test1@test.com", null, null, null, null),
                         new long[] { 1, 2 });
                //Research by phonenumber
                data.Add(BuildParameters(null, null, "0804590101", null, null, null),
                         new long[] { 1, 2 });
                //Research by pickedupdatetimefrom (20190101 - 10h10m10s}
                data.Add(BuildParameters(null, null, null, new DateTime(2019, 01, 01, 10, 10, 10), null, null),
                         new long[] { 2, 3 });
                //Research by pickedupdatetimeto (20180101 - 10h10m10s}
                data.Add(BuildParameters(null, null, null, null, new DateTime(2018, 01, 01, 10, 10, 10), null),
                         new long[] { 1 });
                //Research by reservationstate
                data.Add(BuildParameters(null, null, null, null, null, ReservationState.Booked),
                         new long[] { 3 });
                //Research by All parameters
                data.Add(BuildParameters("Test 1", "test1@test.com", "0804590101", new DateTime(2018, 01, 01, 10, 10, 10), new DateTime(2019, 01, 01, 10, 10, 10), ReservationState.PickedUp),
                         new long[] { 1, 2 });
                //Research by No parameters
                data.Add(BuildParameters(null, null, null, null, null, null),
                         new long[] { 1, 2, 3 });
                return data;
            }
        }

        private CarRentalDbContext GetMockedDbContext()
        {
            var opts = new DbContextOptionsBuilder<CarRentalDbContext>()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString())
                            .Options;
            return new CarRentalDbContext(opts);
        }

        private Reservation BuildReservation(long id, DateTime pickUpDateTime, ReservationState state, long clientId, string clientFullName, string clientEmail, string clientPhoneNumber)
        {
            return new Reservation
            {
                Id = id,
                PickUpDateTime = pickUpDateTime,
                State = state,
                Client = new Client { Id = clientId, FullName = clientFullName, Email = clientEmail, PhoneNumber = clientPhoneNumber }
            };
        }

        private static BrowseReservationsParameters BuildParameters(string clientFullName, string clientEmail, string clientPhoneNumber, DateTime? pickedUpDateTimeFrom, DateTime? pickedUpDateTimeTo, ReservationState? state)
        {
            return new BrowseReservationsParameters()
            {
                ClientFullName = clientFullName,
                ClientEmail = clientEmail,
                ClientPhoneNumber = clientPhoneNumber,
                PickedUpDateTimeFrom = pickedUpDateTimeFrom,
                PickedUpDateTimeTo = pickedUpDateTimeTo,
                ReservationState = state
            };
        }

    }
}
