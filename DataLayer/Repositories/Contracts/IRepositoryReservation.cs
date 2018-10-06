using CarRentalNovility.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Contracts
{
    public interface IRepositoryReservation : IRepository<Reservation>
    {
        Task<Reservation> GetAsync(long id);

        Task<List<Reservation>> GetAllByClientAsync(long clientId);
        Task<List<Reservation>> GetPendingReservationByCarAsync(long id, DateTime pickUpDateTime, DateTime returnDateTime);
        Task<List<Reservation>> BrowseReservationAsync(BrowseReservationsParameters parameters);
    }
}
