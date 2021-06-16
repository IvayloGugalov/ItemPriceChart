using System;
using System.Collections.Generic;
using ItemPriceCharts.Domain.Events;

namespace ItemPriceCharts.Domain.Entities
{
    public class UserAccount : EntityModel
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Username { get; }
        public Email Email { get; private set; }
        public string Password { get; private set; }

        public IReadOnlyCollection<UserAccountOnlineShops> OnlineShopsForUser => this.onlineShopsForUser?.AsReadOnly();
        private readonly List<UserAccountOnlineShops> onlineShopsForUser = new();

        private UserAccount() { }

        public UserAccount(string firstName, string lastName, Email email, string userName, string password)
            : this()
        {
            this.Id = Guid.NewGuid();
            this.FirstName = !string.IsNullOrWhiteSpace(firstName) ? firstName : throw new ArgumentNullException(nameof(firstName));
            this.LastName = !string.IsNullOrWhiteSpace(lastName) ? lastName : throw new ArgumentNullException(nameof(lastName));
            this.Email = !string.IsNullOrWhiteSpace(email.Value) ? email : throw new ArgumentNullException(nameof(email));
            this.Username = !string.IsNullOrWhiteSpace(userName) ? userName : throw new ArgumentNullException(nameof(userName));
            this.Password = !string.IsNullOrWhiteSpace(password) ? password : throw new ArgumentNullException(nameof(password));
        }

        // Return status
        public void AddOnlineShop(OnlineShop onlineShop)
        {
            if (this.onlineShopsForUser == null)
            {
                throw new InvalidOperationException("Could not add an online shop.");
            }

            this.onlineShopsForUser.Add(new UserAccountOnlineShops(this, onlineShop));
            DomainEvents.ShopAdded.Raise(onlineShop);
        }

        public void RemoveOnlineShop(UserAccountOnlineShops onlineShop)
        {
            if (this.onlineShopsForUser == null)
            {
                throw new NullReferenceException("You must use .Include(p => p.OnlineShops) before calling this method.");
            }

            this.onlineShopsForUser.Remove(onlineShop);
            DomainEvents.ShopDeleted.Raise(onlineShop.OnlineShop);
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
