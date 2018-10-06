using CarRentalNovility.BusinessLayer;
using CarRentalNovility.DataLayer;
using CarRentalNovility.Entities;
using CarRentalNovility.Entities.Exceptions;
using DataLayer.Repositories.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class UnitTestBLReservation
    {
        private readonly IUnitOfWork mockedUnitOfWork;

        public UnitTestBLReservation()
        {
            mockedUnitOfWork = new Mock<IUnitOfWork>().Object;
        }

        [Fact]
        public async Task BookCarReservationAsync_ValidReservation_CreatesNewReservation()
        {
            //Arrange
            var carId = 200;
            var clientId = 100;
            var rentalRateFee = 10;
            var depositFee = 0.5m;
            var cancellationRateFee = 5;

            var mockedReservationRepository = GetReservationRepositoryWithoutReservation();
            var mockedCarRepository = GetCarRepositoryWithCar(carId, rentalRateFee, depositFee, cancellationRateFee);
            var mockedClientRepository = GetClientRepositoryWithClient(clientId);
            var bl = new BLReservation(mockedReservationRepository, mockedCarRepository, mockedClientRepository, mockedUnitOfWork);

            var pickupDateTime = DateTime.Now.AddHours(1);
            var returnDateTime = pickupDateTime.AddHours(10).AddMinutes(30);
            var parameters = new BookCarReservationParameters()
            {
                CarId = 200,
                ClientId = 100,
                PickUpDateTime = pickupDateTime,
                ReturnDateTime = returnDateTime
            };

            //Act
            var reservation = await bl.BookCarReservationAsync(parameters);

            //Assert
            Assert.Equal(10.5m, reservation.DurationInHours);
            Assert.Equal(pickupDateTime, reservation.PickUpDateTime);
            Assert.Equal(returnDateTime, reservation.ReturnDateTime);
            Assert.Equal(carId, reservation.Car.Id);
            Assert.Equal(clientId, reservation.Client.Id);
            Assert.Equal(ReservationState.Booked, reservation.State);
            Assert.Equal(reservation.Car.Type.CancellationRateFee, reservation.ClientAccount.CancellationFeeValueAtBookingMoment);
            Assert.Equal(reservation.Car.Type.RentalRateFee, reservation.ClientAccount.RentalRateFeeValueAtBookingMoment);
            Assert.Equal(reservation.DurationInHours * rentalRateFee * depositFee, reservation.ClientAccount.DepositFeePaid);
        }

        [Theory]
        [InlineData(0, 100, 1, 10, false, ErrorCode.UnknownCar)] //not existing car
        [InlineData(200, 0, 1, 10, false, ErrorCode.UnknownClient)] //not existing client
        [InlineData(200, 100, -1, 10, false, ErrorCode.PickUpDateTimeInThePast)] //pickup in the past
        [InlineData(200, 100, 5, 1, false, ErrorCode.ReturnDateTimeBeforePickUpDateTime)] //return before pickup
        [InlineData(200, 100, 1, 10, true, ErrorCode.PendingReservationExists)] //exists pending reservation
        public async Task BookCarReservationAsync_InvalidReservation_ThrowsCustomException(long carId, long clientId, int pickupHours, int returnHours, bool hasReservation, ErrorCode errorCode)
        {
            //Arrange
            var rentalRateFee = 10;
            var depositFee = 0.5m;
            var cancellationRateFee = 5;

            var mockedReservationRepository = hasReservation ? GetReservationRepositoryWithPendingReservation() : GetReservationRepositoryWithoutReservation();
            var mockedCarRepository = GetCarRepositoryWithCar(200, rentalRateFee, depositFee, cancellationRateFee);
            var mockedClientRepository = GetClientRepositoryWithClient(100);
            var bl = new BLReservation(mockedReservationRepository, mockedCarRepository, mockedClientRepository, mockedUnitOfWork);

            var pickupDateTime = DateTime.Now.AddHours(pickupHours);
            var returnDateTime = DateTime.Now.AddHours(returnHours);
            var parameters = new BookCarReservationParameters()
            {
                CarId = carId,
                ClientId = clientId,
                PickUpDateTime = pickupDateTime,
                ReturnDateTime = returnDateTime
            };

            //Act
            async Task act() => await bl.BookCarReservationAsync(parameters);

            //Assert
            var exception = await Assert.ThrowsAsync<CustomException>(act);
            Assert.Equal(errorCode, exception.code);
        }

        [Theory]
        [InlineData(1, 2, ReservationState.Booked, ErrorCode.UnknownReservation)] //reservation not found on db
        [InlineData(1, 1, ReservationState.Cancelled, ErrorCode.InvalidStateTransition)]
        [InlineData(1, 1, ReservationState.PickedUp, ErrorCode.InvalidStateTransition)]
        [InlineData(1, 1, ReservationState.Returned, ErrorCode.InvalidStateTransition)]
        public async Task CancelCarReservationAsync_Invalid_ThrowsCustomException(long reservationIdToCancel, long reservationIdInDb, ReservationState previousState, ErrorCode errorCode)
        {
            //Arrange 
            var mockedReservationRepository = GetReservationRepositoryWithReservation(reservationIdInDb, previousState);
            var bl = new BLReservation(mockedReservationRepository, null, null, mockedUnitOfWork);

            //Act
            async Task actCancel() => await bl.CancelCarReservationAsync(reservationIdToCancel);

            //Assert
            var exception = await Assert.ThrowsAsync<CustomException>(actCancel);
            Assert.Equal(errorCode, exception.code);
        }

        [Theory]
        [InlineData(1, 2, ReservationState.Booked, ErrorCode.UnknownReservation)] //reservation not found on db
        [InlineData(1, 1, ReservationState.Cancelled, ErrorCode.InvalidStateTransition)]
        [InlineData(1, 1, ReservationState.PickedUp, ErrorCode.InvalidStateTransition)]
        [InlineData(1, 1, ReservationState.Returned, ErrorCode.InvalidStateTransition)]
        public async Task PickUpCarAsync_Invalid_ThrowsCustomException(long reservationIdToPickup, long reservationIdInDb, ReservationState previousState, ErrorCode errorCode)
        {
            //Arrange 
            var mockedReservationRepository = GetReservationRepositoryWithReservation(reservationIdInDb, previousState);
            var bl = new BLReservation(mockedReservationRepository, null, null, mockedUnitOfWork);

            //Act
            async Task act() => await bl.PickUpCarAsync(reservationIdToPickup);

            //Assert
            var exception = await Assert.ThrowsAsync<CustomException>(act);
            Assert.Equal(errorCode, exception.code);
        }

        [Theory]
        [InlineData(1, 2, ReservationState.Booked, ErrorCode.UnknownReservation)] //reservation not found on db
        [InlineData(1, 1, ReservationState.Booked, ErrorCode.InvalidStateTransition)]
        [InlineData(1, 1, ReservationState.Cancelled, ErrorCode.InvalidStateTransition)]
        [InlineData(1, 1, ReservationState.Returned, ErrorCode.InvalidStateTransition)]
        public async Task ReturnCarAsync_Invalid_ThrowsCustomException(long reservationIdToReturn, long reservationIdInDb, ReservationState previousState, ErrorCode errorCode)
        {
            //Arrange 
            var mockedReservationRepository = GetReservationRepositoryWithReservation(reservationIdInDb, previousState);
            var bl = new BLReservation(mockedReservationRepository, null, null, mockedUnitOfWork);

            //Act
            async Task act() => await bl.ReturnCarAsync(reservationIdToReturn);

            //Assert
            var exception = await Assert.ThrowsAsync<CustomException>(act);
            Assert.Equal(errorCode, exception.code);
        }

        [Fact]
        public async Task PickUpCarAsync_Valid_UpdatesReservationState()
        {
            //Arrange 
            var mockedReservationRepository = GetReservationRepositoryWithReservation(50, ReservationState.Booked);
            var bl = new BLReservation(mockedReservationRepository, null, null, mockedUnitOfWork);

            //Act 
            var reservation = await bl.PickUpCarAsync(50);

            //Assert
            Assert.Equal(ReservationState.PickedUp, reservation.State);
        }

        [Fact]
        public async Task ReturnCarAsync_Valid_UpdatesReservationStateAndRentalFeePaid()
        {
            //Arrange 
            var mockedReservationRepository = GetReservationRepositoryWithReservation(50, ReservationState.PickedUp);
            var bl = new BLReservation(mockedReservationRepository, null, null, mockedUnitOfWork);

            //Act 
            var reservation = await bl.ReturnCarAsync(50);

            //Assert
            Assert.Equal(ReservationState.Returned, reservation.State);
            Assert.Equal(5, reservation.ClientAccount.RentalFeePaid);
        }

        [Fact]
        public async Task CancelCarReservationAsync_Valid_UpdatesReservationStateAndRentalFeePaid()
        {
            //Arrange 
            var mockedReservationRepository = GetReservationRepositoryWithReservation(50, ReservationState.Booked);
            var bl = new BLReservation(mockedReservationRepository, null, null, mockedUnitOfWork);

            //Act 
            var reservation = await bl.CancelCarReservationAsync(50);

            //Assert
            Assert.Equal(ReservationState.Cancelled, reservation.State);
            Assert.Equal(5, reservation.ClientAccount.CancellationFeePaid);
        }

        #region mock helpers

        private IRepositoryReservation GetReservationRepositoryWithReservation(long desiredReservationId, ReservationState desiredState)
        {
            var mockReservation = new Mock<IRepositoryReservation>();
            mockReservation.Setup(rep => rep.GetAsync(desiredReservationId))
                           .ReturnsAsync(
                                new Reservation()
                                {
                                    Id = desiredReservationId,
                                    State = desiredState,
                                    ClientAccount =
                                        new ClientAccount()
                                        {
                                            RentalRateFeeValueAtBookingMoment = 10,
                                            CancellationFeeValueAtBookingMoment = 1,
                                            DepositFeePaid = 5
                                        },
                                    PickUpDateTime = new DateTime(2018, 01, 01, 9, 0, 0),
                                    ReturnDateTime = new DateTime(2018, 01, 01, 10, 0, 0),
                                });
            return mockReservation.Object;
        }
        private IRepositoryReservation GetReservationRepositoryWithoutReservation()
        {
            var mockReservation = new Mock<IRepositoryReservation>();
            mockReservation.Setup(rep => rep.GetPendingReservationByCarAsync(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                           .ReturnsAsync(new List<Reservation>());
            return mockReservation.Object;
        }

        private IRepositoryReservation GetReservationRepositoryWithPendingReservation()
        {
            var mockReservation = new Mock<IRepositoryReservation>();
            mockReservation.Setup(rep => rep.GetPendingReservationByCarAsync(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                           .ReturnsAsync(
                                new List<Reservation>()
                                {
                                    new Reservation()
                                    {
                                    }
                                });
            return mockReservation.Object;
        }

        private static IRepositoryClient GetClientRepositoryWithClient(int clientId)
        {
            var mockClient = new Mock<IRepositoryClient>();
            mockClient.Setup(rep => rep.GetAsync(clientId))
                .ReturnsAsync(
                    new Client()
                    {
                        Id = clientId,
                        Email = "mail@test.com",
                        FullName = "Name Surname",
                        PhoneNumber = "0804590101"
                    }
                );
            return mockClient.Object;
        }

        private static IRepositoryCar GetCarRepositoryWithCar(int carId, int rentalRateFee, decimal depositFee, int cancellationRateFee)
        {
            var mockCar = new Mock<IRepositoryCar>();
            mockCar.Setup(rep => rep.GetAsync(carId))
                   .ReturnsAsync(
                        new Car()
                        {
                            Id = carId,
                            PlateNumber = "AA00BB",
                            Type = new CarType()
                            {
                                Id = 300,
                                Name = "Punto",
                                RentalRateFee = rentalRateFee,
                                DepositFee = depositFee,
                                CancellationRateFee = cancellationRateFee
                            }
                        }
                );
            return mockCar.Object;
        }
        #endregion
    }
}
