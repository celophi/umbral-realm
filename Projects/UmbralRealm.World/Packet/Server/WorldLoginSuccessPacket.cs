using BinarySerialization;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Server
{
    [PacketOpcodeMapping((ushort)PacketOpcode.WorldLoginSuccess)]
    public class WorldLoginSuccessPacket : IPacket
    {
        /// <summary>
        /// Unknown. Usually zero.
        /// </summary>
        [FieldOrder(0)]
        public uint Unknown1 { get; set; }
    }
}
