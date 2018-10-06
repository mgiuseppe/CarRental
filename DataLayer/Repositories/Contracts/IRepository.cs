using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Contracts
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
        void Insert(T item);
        void Update(T item);
        Task<int> DeleteAsync(T item);
    }
}
