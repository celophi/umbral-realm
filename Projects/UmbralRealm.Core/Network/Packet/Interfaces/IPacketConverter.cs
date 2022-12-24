using UmbralRealm.Core.Security.Interfaces;

namespace UmbralRealm.Core.Network.Packet.Interfaces
{
    /// <summary>
    /// Creates packet models from serialized data.
    /// </summary>
    public interface IPacketConverter
    {
        /// <summary>
        /// Converts a packet model into an array of bytes.
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="cipher"></param>
        /// <returns></returns>
        byte[] Serialize(IPacket packet, ICipher cipher);

        /// <summary>
        /// Creates packet models from a buffer.
        /// </summary>
        /// <param name="buffer">Data that should be deserialized to the concrete packet model.</param>
        /// <returns></returns>
        IList<IPacket> Deserialize(byte[] buffer, ICipher cipher);
    }
}
