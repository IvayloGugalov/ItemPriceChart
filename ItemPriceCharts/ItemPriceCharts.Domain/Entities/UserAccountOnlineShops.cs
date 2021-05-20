using System;

namespace ItemPriceCharts.Domain.Entities
{
    public class UserAccountOnlineShops
    {
        public Guid UserAccountId { get; private set; }
        public Guid OnlineShopId { get; private set; }

        public UserAccount UserAccount { get; private set; }
        public OnlineShop OnlineShop { get; private set; }

        private UserAccountOnlineShops() { }

        public UserAccountOnlineShops(UserAccount userAccount, OnlineShop onlineShop)
            : this()
        {
            this.UserAccount = userAccount;
            this.OnlineShop = onlineShop;
        }
    }
}
