using UmbralRealm.Core.Security;

namespace UmbralRealm.Core.Network.Interfaces
{
    /// <summary>
    /// Contract for defining a structure that can create <see cref="Connection"/> instances.
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Creates a connection with a unique ID that can send and receiving packets.
        /// </summary>
        /// <param name="socketConnection"></param>
        /// <param name="networkCipher"></param>
        /// <returns></returns>
        Connection Create(ISocketConnection socketConnection, NetworkCipher networkCipher);
    }
}
