using System;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly ModelsContext dbContext;

        private bool disposed = false;

        public IRepository<ItemModel> ItemRepository => new Repository<ItemModel>(this.dbContext);

        public IRepository<OnlineShopModel> OnlineShopRepository => new Repository<OnlineShopModel>(this.dbContext);

        public UnitOfWork(ModelsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
