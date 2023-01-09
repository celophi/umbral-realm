namespace UmbralRealm.Core.Utilities.Interfaces
{
    public interface IDataMediator<T>
    {
        /// <summary>
        /// Publishes <see cref="T>"/> to an internal buffered queue.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task Publish(T data);

        /// <summary>
        /// Allows subscribers to recieve instances of <see cref="T"/> from the internal queue.
        /// </summary>
        /// <param name="subscriber"></param>
        public void Subscribe(IDataSubscriber<T> subscriber);
    }
}
