using CarRentalNovility.Entities;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Contracts
{
    public interface IRepositoryCar : IRepository<Car>
    {
        Task<Car> GetAsync(long id);
    }
}
