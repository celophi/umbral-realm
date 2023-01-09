namespace UmbralRealm.Core.Utilities.Interfaces
{
    public interface IDataSubscriber<T>
    {
        /// <summary>
        /// Handles data that was buffered.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task Handle(T data);
    }
}
