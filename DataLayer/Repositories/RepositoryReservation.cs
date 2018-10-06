using CarRentalNovility.DataLayer;
using CarRentalNovility.Entities;
using DataLayer.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class RepositoryReservation : IRepositoryReservation
    {
        private readonly CarRentalDbContext ctx;

        public RepositoryReservation(CarRentalDbContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<Reservation> GetAsync(long id)
        {
            return await ctx.Reservations.Include(r => r.ClientAccount).SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            return await ctx.Reservations.Include(r => r.ClientAccount).ToListAsync();
        }

        public async Task<List<Reservation>> GetAllByClientAsync(long clientId)
        {
            return await ctx.Reservations.Include(r => r.ClientAccount).Where(r => r.Client.Id == clientId).ToListAsync();
        }

        public void Insert(Reservation item)
        {
            ctx.Reservations.Add(item);
        }

        public void Update(Reservation item)
        {
            ctx.Reservations.Update(item);
        }

        public async Task<int> DeleteAsync(Reservation item)
        {
            var itemToRemove = await ctx.Reservations.FirstOrDefaultAsync(c => c.Id == item.Id);
            var itemsRemoved = 0;

            if (itemToRemove != null)
            {
                ctx.Reservations.Remove(itemToRemove);
                itemsRemoved++;
            }

            return itemsRemoved;
        }

        public async Task<List<Reservation>> GetPendingReservationByCarAsync(long id, DateTime pickUpDateTime, DateTime returnDateTime)
        {
            return await ctx.Reservations
                .Where(r =>
                    r.State == ReservationState.Booked && ((r.PickUpDateTime >= pickUpDateTime) && (r.PickUpDateTime <= returnDateTime) || (r.ReturnDateTime >= pickUpDateTime) && (r.ReturnDateTime <= returnDateTime))
                    || r.State == ReservationState.PickedUp)
                .ToListAsync();
        }

        public async Task<List<Reservation>> BrowseReservationAsync(BrowseReservationsParameters parameters)
        {
            var reservations =
                ctx.Reservations
                .Include(r => r.Client)
                .Include(r => r.ClientAccount)
                .Include(r => r.Car)
                .ThenInclude(c => c.Type) as IQueryable<Reservation>;

            if (!String.IsNullOrWhiteSpace(parameters.ClientFullName))
                reservations = reservations.Where(r => parameters.ClientFullName == r.Client.FullName);
            if (!String.IsNullOrWhiteSpace(parameters.ClientEmail))
                reservations = reservations.Where(r => parameters.ClientEmail == r.Client.Email);
            if (!String.IsNullOrWhiteSpace(parameters.ClientPhoneNumber))
                reservations = reservations.Where(r => parameters.ClientPhoneNumber == r.Client.PhoneNumber);
            if (parameters.PickedUpDateTimeFrom != null)
                reservations = reservations.Where(r => parameters.PickedUpDateTimeFrom <= r.PickUpDateTime);
            if (parameters.PickedUpDateTimeTo != null)
                reservations = reservations.Where(r => parameters.PickedUpDateTimeTo >= r.PickUpDateTime);
            if (parameters.ReservationState != null)
                reservations = reservations.Where(r => parameters.ReservationState == r.State);

            return await reservations.ToListAsync();

            /*  Pretty version but i'm not sure about performances on many data
                return await ctx.Reservations
                .Include(r => r.Client)
                .Include(r => r.ClientAccount)
                .Include(r => r.Car)
                .ThenInclude(c => c.Type)
                .Where(r =>
                    (parameters.ClientFullName == null || parameters.ClientFullName == r.Client.FullName)
                    && (parameters.ClientEmail == null || parameters.ClientEmail == r.Client.Email)
                    && (parameters.ClientPhoneNumber == null || parameters.ClientPhoneNumber == r.Client.PhoneNumber)
                    && (parameters.PickedUpDateTimeFrom == null || parameters.PickedUpDateTimeFrom < r.PickUpDateTime)
                    && (parameters.PickedUpDateTimeTo == null || parameters.PickedUpDateTimeTo > r.PickUpDateTime)
                    && (parameters.ReservationState == null || parameters.ReservationState == r.State))
                .ToListAsync();
            */
        }
    }
}
