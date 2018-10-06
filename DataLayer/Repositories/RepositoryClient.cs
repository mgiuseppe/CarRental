using CarRentalNovility.DataLayer;
using CarRentalNovility.Entities;
using DataLayer.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class RepositoryClient : IRepositoryClient
    {
        private readonly CarRentalDbContext ctx;

        public RepositoryClient(CarRentalDbContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<Client> GetAsync(long id)
        {
            return await ctx.Clients.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return await ctx.Clients.ToListAsync();
        }

        public void Insert(Client item)
        {
            ctx.Clients.Add(item);
        }

        public void Update(Client item)
        {
            ctx.Clients.Update(item);
        }

        public async Task<int> DeleteAsync(Client item)
        {
            var itemToRemove = await ctx.Clients.FirstOrDefaultAsync(c => c.Id == item.Id);
            var itemsRemoved = 0;

            if (itemToRemove != null)
            {
                ctx.Clients.Remove(itemToRemove);
                itemsRemoved++;
            }

            return itemsRemoved;
        }
    }
}
