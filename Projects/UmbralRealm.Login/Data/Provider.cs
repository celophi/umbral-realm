using System.Data;
using Dapper;
using UmbralRealm.Login.Dto;

namespace UmbralRealm.Login.Data
{
    public class Provider
    {
        /// <summary>
        /// Used to create connections to the database.
        /// </summary>
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public Provider(IDbConnectionFactory dbConnectionFactory)
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

        /// <summary>
        /// Selects an account by the name.
        /// </summary>
        /// <param name="name">Unique user name for the account.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<AccountDto> SelectByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("name", name, DbType.String);

                using var connection = _dbConnectionFactory.Create();
                return await connection.QueryFirstOrDefaultAsync<AccountDto>("Account_Select_ByName", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
