using System.Threading.Tasks;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public interface IUserAccountService
    {
        Task<UserAccountRegistrationResult> CreateUserAccount(string firstName, string lastName, string email, string userName, string password);
        Task<(UserAccountLoginResult loginResult, UserAccount userAccount)> TryGetUserAccount(string userName, string email, string password);
        Task<bool> DeleteUserAccount(UserAccount userAccount);
    }
}