using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using NLog;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public enum UserAccountRegistrationResult
    {
        UserAccountCreated = 1,
        UserNameAlreadyExists = 2,
        EmailAlreadyExists = 3,
        InvalidPassword = 4,
        CanNotCreateUserAccount = 5
    }

    public enum UserAccountLoginResult
    {
        SuccessfullyLogin = 1,
        InvalidUsernameOrEmail = 2,
        InvalidPassword = 3
    }

    public class UserAccountService : IUserAccountService
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(UserAccountService));

        private readonly IRepository<UserAccount> userAccountRepository;

        public UserAccountService(IRepository<UserAccount> userAccountRepository)
        {
            this.userAccountRepository = userAccountRepository;
        }

        public async Task<UserAccountRegistrationResult> CreateUserAccount(string firstName, string lastName, string email, string userName, string password)
        {
            try
            {
                if (this.IsDuplicate(userAccount => userAccount.Username == userName))
                {
                    return UserAccountRegistrationResult.UserNameAlreadyExists;
                }

                if (this.IsDuplicate(userAccount => userAccount.Email.Value == email))
                {
                    return UserAccountRegistrationResult.EmailAlreadyExists;
                }

                var userAccount = new UserAccount(
                    firstName: firstName,
                    lastName: lastName,
                    email: new Email(email),
                    userName: userName,
                    password: password);

                await this.userAccountRepository.Add(userAccount).ConfigureAwait(false);

                logger.Debug($"Created new account: '{userAccount}'");

                return UserAccountRegistrationResult.UserAccountCreated;
            }
            catch (Exception e)
            {
                logger.Error(e, "Couldn't create account.");

                return UserAccountRegistrationResult.CanNotCreateUserAccount;
            }
        }

        public async Task<bool> DeleteUserAccount(UserAccount userAccount)
        {
            try
            {
                bool userAccountDeleted = false;
                if (this.IsUserAccountExisting(userAccount.Id))
                {
                    userAccountDeleted = await this.userAccountRepository.Delete(userAccount).ConfigureAwait(false);

                    logger.Debug($"Deleted account: '{userAccount}'.");
                }

                return userAccountDeleted;
            }
            catch (Exception e)
            {
                logger.Error(e, $"Couldn't delete account: '{userAccount}'.");
                throw;
            }
        }

        public async Task<(UserAccountLoginResult loginResult, UserAccount userAccount)> TryGetUserAccount(string userName, string email, string password)
        {
            try
            {
                var retrievedUserAccounts = await this.userAccountRepository.GetAll(
                    filter: a => a.Username == userName && a.Email.Value == email).ConfigureAwait(false);

                var userAccount = retrievedUserAccounts.FirstOrDefault();

                if (userAccount != null)
                {
                    if (userAccount.Password == password)
                    {
                        return (loginResult: UserAccountLoginResult.SuccessfullyLogin, userAccount: userAccount);
                    }

                    return (loginResult: UserAccountLoginResult.InvalidPassword, null);
                }

                return (loginResult: UserAccountLoginResult.InvalidUsernameOrEmail, userAccount: null);
            }
            catch (Exception e)
            {
                logger.Error(e, $"Couldn't retrieve user with email: '{email}'.");
                throw;
            }
        }

        private bool IsDuplicate(Expression<Func<UserAccount, bool>> filter) =>
            this.userAccountRepository.IsEntityExistingByAttribute(filter)
                .GetAwaiter().GetResult();

        private bool IsUserAccountExisting(int userAccountId) =>
            this.userAccountRepository.IsExisting(userAccountId).GetAwaiter().GetResult();
    }
}
