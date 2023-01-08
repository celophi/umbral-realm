namespace UmbralRealm.Core.Network.Interfaces
{
    public interface IConnectionSubscriber
    {
        /// <summary>
        /// Handles a connection.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public Task HandleConnection(IWriteConnection connection);
    }
}
