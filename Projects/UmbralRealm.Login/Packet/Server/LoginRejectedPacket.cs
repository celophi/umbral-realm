using BinarySerialization;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Login.Packet.Server
{
    /// <summary>
    /// Authentication failure packet sent from the server to the client on login.
    /// </summary>
    [PacketOpcodeMapping((ushort)PacketOpcode.LoginRejected)]
    public class LoginRejectedPacket : IPacket
    {
        /// <summary>
        /// Unknown, but my guess is that this is some index to the reason of the failure.
        /// </summary>
        [FieldOrder(0)]
        public LoginFailureResult Reason { get; set; }
    }
}
