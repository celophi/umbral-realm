using BinarySerialization;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.World.Packet.Model;

namespace UmbralRealm.World.Packet.Client
{
    [PacketOpcodeMapping((ushort)PacketOpcode.ClientConfiguration)]
    public class ClientConfigurationPacket : IPacket
    {
        [FieldOrder(0)]
        public ushort Unknown1 { get; set; }

        /// <summary>
        /// Number of configuration entries.
        /// </summary>
        [FieldOrder(1)]
        public ushort Count { get; set; }

        /// <summary>
        /// Entries found in the file.
        /// </summary>
        [FieldOrder(2)]
        [FieldCount(nameof(Count))]
        public List<ClientConfigurationEntry> Entries { get; set; } = new();

        /// <summary>
        /// This is probably(?) the maximum number of config hexadecimal values in the hardcoded table loaded on the client.
        /// There is a loop that goes from 0 -> 101, and that's probably what this number is related to.
        /// </summary>
        [FieldOrder(3)]
        public ushort MaxValue { get; set; } = 102;
    }
}
