using System.Data;

namespace UmbralRealm.Login.Data
{
    /// <summary>
    /// Specifies a contract for creating database connections.
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Factory method for creating a database connection.
        /// </summary>
        /// <returns></returns>
        IDbConnection Create();
    }
}
