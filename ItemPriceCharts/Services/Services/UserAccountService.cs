using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
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
        SuccessfulLogin = 1,
        InvalidUsernameOrEmail = 2,
        InvalidPassword = 3
    }

    //We ar using raw sql queries, as EF Core cannot resolve UserAccount.Email,
    //because in C# Email is an object, while in the database it's a string
    public class UserAccountService : IUserAccountService
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(UserAccountService));

        public async Task<UserAccountRegistrationResult> CreateUserAccount(string firstName, string lastName, string email, string userName, string password)
        {
            try
            {
                using (var context = new ModelsContext())
                {
                    var containsDuplicateUsername = context.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccount WHERE Username = '{userName}'").Any();

                    if (containsDuplicateUsername)
                    {
                        return UserAccountRegistrationResult.UserNameAlreadyExists;
                    }

                    var containsDuplicateEmail = context.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccount WHERE Email = '{email}'").Any();

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

                    context.UserAccounts.Attach(userAccount);
                    context.UserAccounts.Add(userAccount);

                    await context.SaveChangesAsync();

                    logger.Debug($"Created new account: '{userAccount}'");

                    return UserAccountRegistrationResult.UserAccountCreated;
                }
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
                using (var context = new ModelsContext())
                {
                    var isUserAccountExisting = await context.UserAccounts.FindAsync(userAccount.Id) != null;

                    if (isUserAccountExisting)
                    {
                        if (context.Entry(userAccount).State == EntityState.Deleted)
                        {
                            context.UserAccounts.Attach(userAccount);
                        }

                        context.UserAccounts.Remove(userAccount);
                        await context.SaveChangesAsync();

                        logger.Debug($"Deleted account: '{userAccount}'.");

                        return true;
                    }

                    return false;
                }
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
                using (var context = new ModelsContext())
                {
                    return await context.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccount WHERE Username = '{userName}' AND Email = '{email}'")
                        .Include("OnlineShopsForAccount")
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);
                }
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
                using (var context = new ModelsContext())
                {
                    var userAccount = await context.UserAccounts.FromSqlRaw($"SELECT * FROM UserAccount WHERE Username = '{userName}' AND Email = '{email}'")
                        .Include("OnlineShopsForAccount")
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);

                    if (userAccount != null)
                    {
                        if (userAccount.Password == password)
                        {
                            logger.Info($"Logged in as username: {userName}");
                            return (loginResult: UserAccountLoginResult.SuccessfulLogin, userAccount: userAccount);
                        }

                        logger.Warn($"Invalid password for username: {userName}");
                        return (loginResult: UserAccountLoginResult.InvalidPassword, null);
                    }

                    logger.Info($"No such user with username: {userName}");
                    return (loginResult: UserAccountLoginResult.InvalidUsernameOrEmail, userAccount: null);
                }
            }
            catch (Exception e)
            {
                logger.Error(e, $"Couldn't retrieve user with email: '{email}'.");
                throw;
            }
        }
    }
}
