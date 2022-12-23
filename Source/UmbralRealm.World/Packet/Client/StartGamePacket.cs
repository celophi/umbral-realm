using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.World.Packet.Client
{
    [PacketOpcodeMapping((ushort)PacketOpcode.StartGame)]
    public class StartGamePacket : IPacket
    {
        /// <summary>
        /// Uniquely identifies the player.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <summary>
        /// The unique (MAC) address of the client's network device.
        /// </summary>
        public string NetworkHardwareAddress { get; set; } = string.Empty;

        /// <summary>
        /// This value appears to be unchanging. It is always 0x01.
        /// </summary>
        public byte Unknown { get; set; } = 1;

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutUInt32(this.PlayerId);
            writer.PutLPString(this.NetworkHardwareAddress);
            writer.PutByte(this.Unknown);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            this.PlayerId = reader.GetUInt32();
            this.NetworkHardwareAddress = reader.GetLPString();
            this.Unknown = reader.GetByte();
        }
    }
}
