using System;
using System.Collections.Generic;
using System.Linq;

namespace ItemPriceCharts.Domain.Entities
{
    public class UserAccount : EntityModel
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Username { get; }
        public Email Email { get; private set; }
        public string Password { get; private set; }

        public IReadOnlyCollection<UserAccountOnlineShops> OnlineShopsForUser => this.onlineShops?.AsReadOnly();
        private readonly List<UserAccountOnlineShops> onlineShops = new();

        private UserAccount() { }

        public UserAccount(string firstName, string lastName, Email email, string userName, string password, ICollection<OnlineShop> onlineShops)
            : this()
        {
            this.Id = Guid.NewGuid();
            this.FirstName = !string.IsNullOrWhiteSpace(firstName) ? firstName : throw new ArgumentNullException(nameof(firstName));
            this.LastName = !string.IsNullOrWhiteSpace(lastName) ? lastName : throw new ArgumentNullException(nameof(lastName));
            this.Email = !string.IsNullOrWhiteSpace(email.Value) ? email : throw new ArgumentNullException(nameof(email));
            this.Username = !string.IsNullOrWhiteSpace(userName) ? userName : throw new ArgumentNullException(nameof(userName));
            this.Password = !string.IsNullOrWhiteSpace(password) ? password : throw new ArgumentNullException(nameof(password));

            this.onlineShops = new List<UserAccountOnlineShops>(onlineShops?.Select(x => new UserAccountOnlineShops(this, x)));
        }

        // Return status
        public UserAccountOnlineShops AddOnlineShop(OnlineShop onlineShop)
        {
            if (this.onlineShops != null)
            {
                var userAccountOnlineShop = new UserAccountOnlineShops(this, onlineShop);
                this.onlineShops.Add(userAccountOnlineShop);

                return userAccountOnlineShop;
            }

            throw new InvalidOperationException("Could not add an online shop.");
        }

        public void RemoveOnlineShop(OnlineShop onlineShop)
        {
            if (this.onlineShops == null)
            {
                throw new NullReferenceException("You must use .Include(p => p.OnlineShops) before calling this method.");
            }

            var onlineShopForAccount = this.onlineShops.SingleOrDefault(x => x.OnlineShop == onlineShop);
            this.onlineShops.Remove(onlineShopForAccount);
        }

        public override string ToString()
        {
            return $"Id: {this.Id}, Username: {this.Username}, Email: {this.Email}";
        }

        public override bool Equals(object obj)
        {
            if (obj is not UserAccount other)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.Email.Value != other.Email.Value)
            {
                return false;
            }

            if (this.Username != other.Username)
            {
                return false;
            }

            return this.Password == other.Password;
        }

        public static bool operator ==(UserAccount a, UserAccount b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(UserAccount a, UserAccount b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
