using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CarRentalNovility.DataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        public UnitOfWork(CarRentalDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CompleteAsync() => await context.SaveChangesAsync();
    }
}
