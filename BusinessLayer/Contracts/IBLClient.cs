using CarRentalNovility.Entities;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts
{
    public interface IBLClient
    {
        Task<ClientAccount> GetBalanceAsync(long clientID);
    }
}