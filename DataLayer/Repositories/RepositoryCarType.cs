using CarRentalNovility.DataLayer;
using CarRentalNovility.Entities;
using DataLayer.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class RepositoryCarType : IRepositoryCarType
    {
        private readonly CarRentalDbContext ctx;

        public RepositoryCarType(CarRentalDbContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<CarType> GetAsync(int id)
        {
            return await ctx.CarTypes.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<CarType>> GetAllAsync()
        {
            return await ctx.CarTypes.ToListAsync();
        }

        public void Insert(CarType item)
        {
            ctx.CarTypes.Add(item);
        }

        public void Update(CarType item)
        {
            ctx.CarTypes.Update(item);
        }

        public async Task<int> DeleteAsync(CarType item)
        {
            var itemToRemove = await ctx.CarTypes.FirstOrDefaultAsync(c => c.Id == item.Id);
            var itemsRemoved = 0;

            if (itemToRemove != null)
            {
                ctx.CarTypes.Remove(itemToRemove);
                itemsRemoved++;
            }

            return itemsRemoved;
        }
    }
}
