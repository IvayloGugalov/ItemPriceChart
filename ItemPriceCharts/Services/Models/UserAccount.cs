using System;
using System.Collections.Generic;

namespace ItemPriceCharts.Services.Models
{
    public class UserAccount : EntityModel
    {
        public string FirstName { get; }
        public string LastName { get; }
        public Email Email { get; private set; }
        public string Username { get; }
        public string Password { get; private set; }

        public IReadOnlyCollection<OnlineShop> OnlineShopsForAccount => this.onlineShopsForAccount.AsReadOnly();
        private readonly List<OnlineShop> onlineShopsForAccount = new List<OnlineShop>();

        private UserAccount() { }

        public UserAccount(string firstName, string lastName, Email email, string userName, string password) : this()
        {
            this.FirstName = !string.IsNullOrWhiteSpace(firstName) ? firstName : throw new ArgumentNullException(nameof(firstName));
            this.LastName = !string.IsNullOrWhiteSpace(lastName) ? lastName : throw new ArgumentNullException(nameof(lastName));
            this.Email = !string.IsNullOrWhiteSpace(email.Value) ? email : throw new ArgumentNullException(nameof(email));
            this.Username = !string.IsNullOrWhiteSpace(userName) ? userName : throw new ArgumentNullException(nameof(userName));
            this.Password = !string.IsNullOrWhiteSpace(password) ? password : throw new ArgumentNullException(nameof(password));
        }

        public void AddOnlineShop(OnlineShop onlineShop)
        {
            this.onlineShopsForAccount.Add(onlineShop);
        }

        public void RemoveOnlineShop(OnlineShop onlineShop)
        {
            this.onlineShopsForAccount.Remove(onlineShop);
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
            if (this.Password != other.Password)
            {
                return false;
            }

            return true;
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
