namespace UmbralRealm.Core.Network.Interfaces
{
    public interface IConnectionMediator
    {
        /// <summary>
        /// Publishes connection instances to an internal queue.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public Task Publish(IWriteConnection connection);

        /// <summary>
        /// Allows subscribers to recieve instances of connections.
        /// </summary>
        /// <param name="subscriber"></param>
        public void Subscribe(IConnectionSubscriber subscriber);
    }
}
