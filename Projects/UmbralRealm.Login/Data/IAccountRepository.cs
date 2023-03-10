using UmbralRealm.Domain.Entities;
using UmbralRealm.Domain.ValueObjects;

namespace UmbralRealm.Login.Data
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Selects an account by the name.
        /// </summary>
        /// <param name="username">Unique user name for the account.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<AccountEntity> GetByUsername(Username username);
    }
}
