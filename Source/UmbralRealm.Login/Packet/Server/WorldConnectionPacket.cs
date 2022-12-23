using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Login.Packet.Server
{
    /// <summary>
    /// World IP address and port information sent from the server to the client upon authenticating to the world server.
    /// </summary>
    [PacketOpcodeMapping((ushort)PacketOpcode.WorldConnection)]
    public class WorldConnectionPacket : IPacket
    {
        /// <summary>
        /// Unknown. Seems unused?
        /// </summary>
        public uint Unknown1 { get; set; }

        public uint Unknown2 { get; set; }

        public uint WorldIPAddress { get; set; }

        public ushort WorldIPPort { get; set; }

        public byte[] Unknown3 { get; set; } = new byte[8];

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutUInt32(this.Unknown1);
            writer.PutUInt32(this.Unknown2);
            writer.PutUInt32(this.WorldIPAddress);
            writer.PutUInt16(this.WorldIPPort);
            writer.PutBytes(this.Unknown3);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.Unknown1 = reader.GetUInt32();
            this.Unknown2 = reader.GetUInt32();
            this.WorldIPAddress = reader.GetUInt32();
            this.WorldIPPort = reader.GetUInt16();
            this.Unknown3 = reader.GetBytes(8);
        }
    }
}
