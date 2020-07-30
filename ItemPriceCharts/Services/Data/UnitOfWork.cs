using System;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly ModelsContext dbContext;
        private readonly Repository<ItemModel> itemRepository;
        private readonly Repository<OnlineShopModel> shopRepository;

        private bool disposed = false;

        public Repository<ItemModel> ItemRepository => this.itemRepository ?? new Repository<ItemModel>(this.dbContext);

        public Repository<OnlineShopModel> OnlineShopRepository => this.shopRepository ?? new Repository<OnlineShopModel>(this.dbContext);

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
