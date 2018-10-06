using CarRentalNovility.Entities;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Contracts
{
    public interface IRepositoryClient : IRepository<Client>
    {
        Task<Client> GetAsync(long id);
    }
}
