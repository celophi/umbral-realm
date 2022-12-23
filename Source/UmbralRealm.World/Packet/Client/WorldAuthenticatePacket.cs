using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Client
{
    [PacketOpcodeMapping((ushort)PacketOpcode.WorldAuthenticate)]
    public class WorldAuthenticatePacket : IPacket
    {
        public uint Unknown1 { get; set; }
        public uint Unknown2 { get; set; }

        /// <summary>
        /// IP address of the world the client is connecting to.
        /// </summary>
        public uint WorldIPAddress { get; set; }

        /// <summary>
        /// Port of the world the client is connecting to.
        /// </summary>
        public ushort WorldIPPort { get; set; }

        public uint Unknown3 { get; set; }

        public uint Unknown4 { get; set; }

        /// <summary>
        /// Hardware address of client's network device.
        /// </summary>
        public string NetworkHardwareAddress { get; set; } = string.Empty;

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutUInt32(this.Unknown1);
            writer.PutUInt32(this.Unknown2);
            writer.PutUInt32(this.WorldIPAddress);
            writer.PutUInt16(this.WorldIPPort);
            writer.PutUInt32(this.Unknown3);
            writer.PutUInt32(this.Unknown4);
            writer.PutLPString(this.NetworkHardwareAddress);

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
            this.Unknown3 = reader.GetUInt32();
            this.Unknown4 = reader.GetUInt32();
            this.NetworkHardwareAddress = reader.GetLPString();
        }
    }
}
