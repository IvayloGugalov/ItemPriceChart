using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using NLog;

using ItemPriceCharts.Domain.Entities;

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
        private static readonly Logger logger = LogManager.GetLogger(nameof(UserAccountService));

        public async Task<UserAccountRegistrationResult> CreateUserAccount(string firstName, string lastName, string email, string userName, string password)
        {
            try
            {
                using ModelsContext dbContext = new();

                var containsDuplicateUsername = dbContext.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccount WHERE Username = '{userName}'").Any();

                if (containsDuplicateUsername)
                {
                    return UserAccountRegistrationResult.UserNameAlreadyExists;
                }

                var containsDuplicateEmail = dbContext.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccount WHERE Email = '{email}'").Any();

                if (containsDuplicateEmail)
                {
                    return UserAccountRegistrationResult.EmailAlreadyExists;
                }

                dbContext.BeginTransaction();

                var userAccount = new UserAccount(
                    firstName: firstName,
                    lastName: lastName,
                    email: new Email(email),
                    userName: userName,
                    password: password,
                    onlineShops: new List<OnlineShop>());

                dbContext.UserAccounts.Attach(userAccount);
                await dbContext.UserAccounts.AddAsync(userAccount).ConfigureAwait(false);

                dbContext.CommitToDatabase();

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
                using ModelsContext dbContext = new();

                var isUserAccountExisting = await dbContext.UserAccounts.FindAsync(userAccount.Id) != null;

                if (isUserAccountExisting)
                {
                    if (dbContext.UserAccounts.Attach(userAccount).State == EntityState.Deleted)
                    {
                        dbContext.UserAccounts.Attach(userAccount);
                    }

                    dbContext.UserAccounts.Remove(userAccount);
                    dbContext.CommitToDatabase();

                    logger.Debug($"Deleted account: '{userAccount}'.");

                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                logger.Error(e, $"Couldn't delete account: '{userAccount}'.");
                throw;
            }
        }

        public async Task<UserAccount> GetUserAccount(string userName, string email)
        {
            try
            {
                using ModelsContext dbContext = new();

                var userAccount = await dbContext.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccount WHERE Username = '{userName}' AND Email = '{email}'")
                    .Include(u => u.OnlineShopsForUser)
                    .ThenInclude(s => s.OnlineShop)
                    .ThenInclude(o => o.Items)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);
                return userAccount;
            }
            catch (Exception e)
            {
                logger.Error(e, $"Couldn't retrieve user with email: '{email}'.");
                throw;
            }
        }

        public async Task<(UserAccountLoginResult loginResult, UserAccount userAccount)> TryGetUserAccount(string userName, string email, string password)
        {
            try
            {
                using ModelsContext dbContext = new();

                var usernameFound = dbContext.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccount WHERE Username = '{userName}'").Any();

                if (!usernameFound)
                {
                    logger.Info($"No such user with username: {userName}");
                    return (UserAccountLoginResult.InvalidUsername, null);
                }

                var emailFound = dbContext.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccount WHERE Email = '{email}'").Any();

                if (!emailFound)
                {
                    logger.Info($"No such user with email: {email}");
                    return (UserAccountLoginResult.InvalidEmail, null);
                }

                var userAccount = await dbContext.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccount WHERE Username = '{userName}' AND Email = '{email}'")
                    .Include(u => u.OnlineShopsForUser)
                    .ThenInclude(s => s.OnlineShop)
                    .ThenInclude(o => o.Items)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                if (userAccount.Password == password)
                {
                    logger.Info($"Logged in as user: {userName}");
                    return (loginResult: UserAccountLoginResult.SuccessfulLogin, userAccount: userAccount);
                }

                logger.Warn($"Invalid password for user: {userName}");
                return (loginResult: UserAccountLoginResult.InvalidPassword, null);
            }
            catch (Exception e)
            {
                logger.Error(e, $"Couldn't retrieve user with email: '{email}'.");
                throw;
            }
        }
    }
}
