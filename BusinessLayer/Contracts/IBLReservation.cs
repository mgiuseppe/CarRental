using CarRentalNovility.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts
{
    public interface IBLReservation
    {
        Task<Reservation> BookCarReservationAsync(BookCarReservationParameters parameters);
        Task<Reservation> CancelCarReservationAsync(long reservationId);
        Task<Reservation> PickUpCarAsync(long reservationId);
        Task<Reservation> ReturnCarAsync(long reservationId);
        Task<List<Reservation>> BrowseReservationAsync(BrowseReservationsParameters parameters);
    }
}