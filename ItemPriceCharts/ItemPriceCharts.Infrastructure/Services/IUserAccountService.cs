using System.Threading.Tasks;

using ItemPriceCharts.Domain.Entities;

namespace ItemPriceCharts.Infrastructure.Services
{
    public interface IUserAccountService
    {
        Task<UserAccountRegistrationResult> CreateUserAccount(string firstName, string lastName, string email, string userName, string password);
        Task<bool> DeleteUserAccount(UserAccount userAccount);
        Task<UserAccount> GetUserAccount(string userName, string email);
        Task<(UserAccountLoginResult loginResult, UserAccount userAccount)> TryGetUserAccount(string userName, string email, string password);
    }
}