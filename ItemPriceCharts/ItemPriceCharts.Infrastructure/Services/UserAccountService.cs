using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Infrastructure.Data;
using ItemPriceCharts.XmReaderWriter.User;

namespace ItemPriceCharts.Infrastructure.Services
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
        SuccessfulLogin = 1,
        InvalidUsername = 2,
        InvalidEmail = 3,
        InvalidPassword = 4
    }

    // We ar using raw sql queries, as EF Core cannot resolve UserAccount.Email,
    // because in C# Email is an object, while in the database it's a string
    public class UserAccountService : IUserAccountService
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(UserAccountService));

        public async Task<UserAccountRegistrationResult> CreateUserAccount(string firstName, string lastName, string email, string userName, string password)
        {
            try
            {
                using ModelsContext dbContext = new();

                var containsDuplicateUsername = dbContext.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccounts WHERE Username = '{userName}'").Any();

                if (containsDuplicateUsername)
                {
                    return UserAccountRegistrationResult.UserNameAlreadyExists;
                }

                var containsDuplicateEmail = dbContext.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccounts WHERE Email = '{email}'").Any();

                if (containsDuplicateEmail)
                {
                    return UserAccountRegistrationResult.EmailAlreadyExists;
                }

                var userAccount = new UserAccount(
                    firstName: firstName,
                    lastName: lastName,
                    email: new Email(email),
                    userName: userName,
                    password: password);

                dbContext.UserAccounts.Attach(userAccount);
                await dbContext.UserAccounts.AddAsync(userAccount).ConfigureAwait(false);

                await dbContext.SaveChangesAsync();

                Logger.Debug($"Created new account: '{userAccount}'");

                return UserAccountRegistrationResult.UserAccountCreated;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Couldn't create account.");

                return UserAccountRegistrationResult.CanNotCreateUserAccount;
            }
        }

        public async Task<bool> DeleteUserAccount(UserAccount userAccount)
        {
            try
            {
                using ModelsContext dbContext = new();

                var isUserAccountExisting = await dbContext.UserAccounts.FindAsync(userAccount.Id) != null;

                if (isUserAccountExisting)
                {
                    if (dbContext.UserAccounts.Attach(userAccount).State == EntityState.Deleted)
                    {
                        dbContext.UserAccounts.Attach(userAccount);
                    }

                    dbContext.UserAccounts.Remove(userAccount);
                    await dbContext.SaveChangesAsync();

                    Logger.Debug($"Deleted account: '{userAccount}'.");

                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Couldn't delete account: '{userAccount}'.");
                throw;
            }
        }

        public async Task<UserAccount> GetUserAccount(string userName, string email)
        {
            try
            {
                using ModelsContext dbContext = new();

                var userAccount = await dbContext.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccounts WHERE Username = '{userName}' AND Email = '{email}'")
                    .Include(u => u.OnlineShopsForUser)
                    .ThenInclude(u => u.OnlineShop)
                    .ThenInclude(o => o.Items)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                return userAccount;
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Couldn't retrieve user with email: '{email}'.");
                throw;
            }
        }

        public async Task<(UserAccountLoginResult loginResult, UserAccount userAccount)> TryGetUserAccount(string userName, string email, string password)
        {
            try
            {
                using ModelsContext dbContext = new();

                var usernameFound = dbContext.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccounts WHERE Username = '{userName}'").Any();

                if (!usernameFound)
                {
                    Logger.Info($"No such user with username: {userName}");
                    return (UserAccountLoginResult.InvalidUsername, null);
                }

                var emailFound = dbContext.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccounts WHERE Email = '{email}'").Any();

                if (!emailFound)
                {
                    Logger.Info($"No such user with email: {email}");
                    return (UserAccountLoginResult.InvalidEmail, null);
                }

                var userAccount = await dbContext.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccounts WHERE Username = '{userName}' AND Email = '{email}'")
                    .Include(u => u.OnlineShopsForUser)
                    .ThenInclude(u => u.OnlineShop)
                    .ThenInclude(o => o.Items)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                if (userAccount.Password != password)
                {
                    Logger.Warn($"Invalid password for user: {userName}");
                    return (loginResult: UserAccountLoginResult.InvalidPassword, null);
                }

                Logger.Info($"Logged in as user: {userName}");
                return (loginResult: UserAccountLoginResult.SuccessfulLogin, userAccount);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Couldn't retrieve user with email: '{email}'.");
                throw;
            }
        }

        public async Task WriteUserCredentials(UserAccount userAccount, bool userWantsToAutoLogin, string expiryDate)
        {
            try
            {
                await Task.Run(() =>
                {
                    UserCredentialsSettings.Username = userAccount.Username;
                    UserCredentialsSettings.Email = userAccount.Email.Value;
                    UserCredentialsSettings.RememberAccount = userWantsToAutoLogin.ToString();
                    UserCredentialsSettings.LoginExpiresDate = expiryDate;
                    UserCredentialsSettings.WriteToXmlFile();
                });
            }
            catch (Exception e)
            {
                Logger.Error(e, "Can't save user credentials to User settings file.");
            }
        }
    }
}
