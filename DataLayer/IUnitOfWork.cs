using System.Threading.Tasks;

namespace CarRentalNovility.DataLayer
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
    }
}