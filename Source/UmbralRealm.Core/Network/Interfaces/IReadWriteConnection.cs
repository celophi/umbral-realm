namespace UmbralRealm.Core.Network.Interfaces
{
    public interface IReadWriteConnection : IWriteConnection
    {
        /// <summary>
        /// Receives any buffered bytes and places them on the internal queue to be retrieved using <see cref="TryGetPacket(out IPacket)"/>
        /// </summary>
        /// <returns></returns>
        Task ReceiveAsync();
    }
}
