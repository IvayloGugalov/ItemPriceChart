using System;

namespace ItemPriceCharts.Services.Models
{
    public class User : EntityModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public User(string username, string email,  string firstName, string lastName, string password)
        {
            this.Username = !string.IsNullOrWhiteSpace(username) ? username : throw new ArgumentNullException(nameof(username));
            this.Email = !string.IsNullOrWhiteSpace(email) ? email : throw new ArgumentNullException(nameof(email));
            this.Password = !string.IsNullOrWhiteSpace(firstName) ? firstName : throw new ArgumentNullException(nameof(firstName));
            this.FirstName = !string.IsNullOrWhiteSpace(lastName) ? lastName : throw new ArgumentNullException(nameof(lastName));
            this.LastName = !string.IsNullOrWhiteSpace(password) ? password : throw new ArgumentNullException(nameof(password));
        }

        public override string ToString()
        {
            return $"User {this.Username}, with Email {this.Email}";
        }
    }
}
