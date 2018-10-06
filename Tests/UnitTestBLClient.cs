using CarRentalNovility.BusinessLayer;
using CarRentalNovility.Entities;
using CarRentalNovility.Entities.Exceptions;
using DataLayer.Repositories.Contracts;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class UnitTestBLClient
    {
        private IRepositoryReservation mockedReservationRepository;
        private IRepositoryClient mockedClientRepository;

        public UnitTestBLClient()
        {
            SetupMocks();
        }

        private void SetupMocks()
        {
            var mockReservationRepository = new Mock<IRepositoryReservation>();

            mockReservationRepository
                .Setup(rep => rep.GetAllByClientAsync(10))
                .ReturnsAsync(
                    new List<Reservation>()
                    {
                        new Reservation()
                        {
                            ClientAccount = new ClientAccount() { DepositFeePaid = 100, CancellationFeePaid = 100, RentalFeePaid = 100 }
                        },
                        new Reservation()
                        {
                            ClientAccount = new ClientAccount() { DepositFeePaid = 10, CancellationFeePaid = 10, RentalFeePaid = 10 }
                        }
                    }
                );
            mockReservationRepository
                .Setup(rep => rep.GetAllByClientAsync(20))
                .ReturnsAsync(
                    new List<Reservation>()
                );

            mockedReservationRepository = mockReservationRepository.Object;

            var mockClientRepository = new Mock<IRepositoryClient>();
            mockClientRepository
                .Setup(rep => rep.GetAsync(10))
                .ReturnsAsync(new Client() { Id = 10});
            mockClientRepository
                .Setup(rep => rep.GetAsync(20))
                .ReturnsAsync(new Client() { Id = 20 });
            mockedClientRepository = mockClientRepository.Object;
        }

        [Fact]
        public async void GetBalance_NotExistingClient_ThrowsException()
        {
            //Arrange 
            var bl = new BLClient(mockedReservationRepository, mockedClientRepository);
            var clientId = 5;

            //Act 
            async Task act () => await bl.GetBalanceAsync(clientId);

            //Assert
            await Assert.ThrowsAsync<CustomException>(act);
        }

        [Fact]
        public async void GetBalance_ClientWithReservations_AccountValuesGreaterThanZero() //MethodName_ScenarioDescription_ExpectedBehaviour
        {
            //Arrange 
            var bl = new BLClient(mockedReservationRepository, mockedClientRepository);
            var clientId = 10;

            //Act 
            var balance = await bl.GetBalanceAsync(clientId);

            //Assert
            Assert.Equal(110, balance.DepositFeePaid);
            Assert.Equal(110, balance.CancellationFeePaid);
            Assert.Equal(110, balance.RentalFeePaid);
        }

        [Fact]
        public async void GetBalance_ClientWithoutReservations_AccountValuesEqualToZero()
        {
            //Arrange 
            var bl = new BLClient(mockedReservationRepository, mockedClientRepository);
            var clientId = 20;

            //Act 
            var balance = await bl.GetBalanceAsync(clientId);

            //Assert
            Assert.Equal(0, balance.DepositFeePaid);
            Assert.Equal(0,balance.CancellationFeePaid);
            Assert.Equal(0,balance.RentalFeePaid);
        }
    }
}
