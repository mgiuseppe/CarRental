using CarRentalNovility.DataLayer;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class UnitTestRepositoryReservation
    {
        [Fact]
        public async Task BrowseReservationAsync()
        {

        }

        private void GetMockedDbContext()
        {
            var mockdb = new Mock<CarRentalDbContext>();

            //mockdb.Setup(db => db.Reservations); //TODO* perchè non mi fa includere ef core?
        }
    }
}
