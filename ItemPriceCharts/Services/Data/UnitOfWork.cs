using System;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly ModelsContext dbContext;

        private bool disposed = false;

        public IRepository<ItemModel> ItemRepository { get; }

        public IRepository<OnlineShopModel> OnlineShopRepository { get; }

        public IRepository<ItemPrice> ItemPriceRepository { get; }

        public UnitOfWork(ModelsContext dbContext)
        {
            this.dbContext = dbContext;
            this.ItemPriceRepository = new Repository<ItemPrice>(this.dbContext);
            this.ItemRepository = new Repository<ItemModel>(this.dbContext);
            this.OnlineShopRepository = new Repository<OnlineShopModel>(this.dbContext);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChangesAsync();
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
