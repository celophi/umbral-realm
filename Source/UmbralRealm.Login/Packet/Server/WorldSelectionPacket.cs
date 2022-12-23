using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Login.Packet.Server
{
    /// <summary>
    /// World selection information and availability sent from the server to the client.
    /// </summary>
    [PacketOpcodeMapping((ushort)PacketOpcode.WorldSelection)]
    public class WorldSelectionPacket : IPacket
    {
        /// <summary>
        /// Number of worlds that are available.
        /// </summary>
        public ushort WorldCount { get; set; }

        /// <summary>
        /// Server and channel information for connecting to a world.
        /// </summary>
        public readonly List<WorldSelectionInfo> WorldSelectionInfoList = new();

        /// <summary>
        /// World ID that is highlighted by default.
        /// </summary>
        public ushort DefaultWorldId { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        public byte Unknown2 { get; set; }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutUInt16(this.WorldCount);
            
            foreach (var info in this.WorldSelectionInfoList)
            {
                writer.PutBytes(info.Serialize());
            }

            writer.PutUInt16(this.DefaultWorldId);
            writer.PutByte(this.Unknown2);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.WorldCount = reader.GetUInt16();

            for (var i = 0; i < this.WorldCount; i++)
            {
                var info = new WorldSelectionInfo();
                info.Deserialize(reader);
                this.WorldSelectionInfoList.Add(info);
            }

            this.DefaultWorldId = reader.GetUInt16();
            this.Unknown2 = reader.GetByte();
        }
    }
}
