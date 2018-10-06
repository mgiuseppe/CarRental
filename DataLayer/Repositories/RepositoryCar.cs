using System.Collections.Generic;
using System.Threading.Tasks;
using CarRentalNovility.DataLayer;
using CarRentalNovility.Entities;
using DataLayer.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class RepositoryCar : IRepositoryCar
    {
        private readonly CarRentalDbContext ctx;

        public RepositoryCar(CarRentalDbContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<Car> GetAsync(long id)
        {
            return await ctx.Cars.Include(c => c.Type).SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Car>> GetAllAsync()
        {
            return await ctx.Cars.Include(c => c.Type).ToListAsync();
        }

        public void Insert(Car item)
        {
            ctx.Cars.Add(item);
        }

        public void Update(Car item)
        {
            ctx.Cars.Update(item);
        }

        public async Task<int> DeleteAsync(Car item)
        {
            var itemToRemove = await ctx.Cars.FirstOrDefaultAsync(c => c.Id == item.Id);
            var itemsRemoved = 0;

            if(itemToRemove != null)
            {
                ctx.Cars.Remove(itemToRemove);
                itemsRemoved++;
            }

            return itemsRemoved;
        }
    }
}
