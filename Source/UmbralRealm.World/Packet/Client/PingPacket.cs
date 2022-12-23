using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Client
{
    [PacketOpcodeMapping((ushort)PacketOpcode.Ping)]
    public class PingPacket : IPacket
    {
        /// <summary>
        /// Account name.
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// Name of the world.
        /// </summary>
        public string WorldName { get; set; } = string.Empty;

        /// <summary>
        /// All zeros.
        /// </summary>
        public byte[] Unknown1 { get; set; } = new byte[6];

        /// <summary>
        /// Client or server version string?
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Unknown.
        /// </summary>
        public uint Unknown2 { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        public byte[] Unknown3 { get; set; } = new byte[6];

        /// <summary>
        /// Seems like it is supposed to be something like "Windows7-64bit".
        /// </summary>
        public string WindowsVersion { get; set; } = string.Empty;

        /// <summary>
        /// ID of the world connected to.
        /// </summary>
        public ushort WorldId { get; set; }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutLPString(this.Account);
            writer.PutLPString(this.WorldName);
            writer.PutBytes(this.Unknown1);
            writer.PutLPString(this.Version);
            writer.PutUInt32(this.Unknown2);
            writer.PutBytes(this.Unknown3);
            writer.PutLPString(this.WindowsVersion);
            writer.PutUInt16(this.WorldId);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.Account = reader.GetLPString();
            this.WorldName = reader.GetLPString();
            this.Unknown1 = reader.GetBytes(this.Unknown1.Length);
            this.Version = reader.GetLPString();
            this.Unknown2 = reader.GetUInt32();
            this.Unknown3 = reader.GetBytes(this.Unknown3.Length);
            this.WindowsVersion = reader.GetLPString();
            this.WorldId = reader.GetUInt16();
        }
    }
}
