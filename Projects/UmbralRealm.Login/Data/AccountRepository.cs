using System.Data;
using Dapper;
using UmbralRealm.Domain.Entities;
using UmbralRealm.Domain.ValueObjects;

namespace UmbralRealm.Login.Data
{
    public class AccountRepository : IAccountRepository
    {
        /// <summary>
        /// Used to create connections to the database.
        /// </summary>
        private readonly IDbConnectionFactory _dbConnectionFactory;

        /// <summary>
        /// Creates a repository to interact with the database and account information.
        /// </summary>
        /// <param name="dbConnectionFactory"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AccountRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="name">Unique user name for the account.</param>
        /// <param name="password">Password for the account.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<int?> Create(string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("name", name, DbType.String);
                parameters.Add("password", password, DbType.String);

                using var connection = _dbConnectionFactory.Create();
                return await connection.ExecuteAsync("Account_Insert", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<AccountEntity> GetByUsername(Username username)
        {
            ArgumentNullException.ThrowIfNull(username);

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("username", username.Value, DbType.String);

                using var connection = _dbConnectionFactory.Create();
                return await connection.QueryFirstOrDefaultAsync<AccountEntity>("Account_GetByUsername", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
