using BusinessLayer.Contracts;
using CarRentalNovility.Entities;
using CarRentalNovility.Entities.Exceptions;
using DataLayer.Repositories.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalNovility.BusinessLayer
{
    public class BLClient : IBLClient
    {
        private readonly IRepositoryReservation reservationRep;
        private readonly IRepositoryClient clientRep;

        public BLClient(IRepositoryReservation reservationRep, IRepositoryClient clientRep)
        {
            this.reservationRep = reservationRep;
            this.clientRep = clientRep;
        }

        public async Task<ClientAccount> GetBalanceAsync(long clientID)
        {
            await Validate(clientID);

            var accounts = (await reservationRep.GetAllByClientAsync(clientID)).Select(r => r.ClientAccount);
            var balance = new ClientAccount
            {
                DepositFeePaid = accounts.Sum(a => a.DepositFeePaid),
                RentalFeePaid = accounts.Sum(a => a.RentalFeePaid),
                CancellationFeePaid = accounts.Sum(a => a.CancellationFeePaid)
            };

            return balance;
        }

        private async Task Validate(long clientID)
        {
            if (await clientRep.GetAsync(clientID) == null)
                throw new CustomException($"Unknown Client - Id {clientID}", ErrorCode.UnknownClient);
        }
    }
}
