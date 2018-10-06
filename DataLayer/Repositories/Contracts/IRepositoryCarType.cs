using CarRentalNovility.Entities;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Contracts
{
    public interface IRepositoryCarType : IRepository<CarType>
    {
        Task<CarType> GetAsync(int id);
    }
}
