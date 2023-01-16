using System.Data;
using System.Data.SqlClient;

namespace UmbralRealm.Login.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        /// <summary>
        /// Connection string to a database.
        /// </summary>
        private readonly string _connectionString;

        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Returns a new connection.
        /// </summary>
        /// <returns></returns>
        public IDbConnection Create()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
