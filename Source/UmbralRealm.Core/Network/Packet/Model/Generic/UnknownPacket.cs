using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Core.Network.Packet.Model.Generic
{
    /// <summary>
    /// Represents a packet that has not been identified.
    /// </summary>
    public class UnknownPacket : IPacket
    {
        /// <inheritdoc/>
        public ushort Opcode { get; set; }

        /// <summary>
        /// Indicates the server type if applicable.
        /// </summary>
        public ServerType ServerType { get; set; }

        /// <summary>
        /// Indicates the origin of the packet if applicable.
        /// </summary>
        public PacketOrigin Origin { get; set; }

        /// <summary>
        /// Unknown data.
        /// </summary>
        public byte[] Data { get; set; } = Array.Empty<byte>();

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var writer = new BinaryStreamWriter();

            writer.PutUInt16(this.Opcode);
            writer.PutBytes(this.Data);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void Deserialize(BinaryStreamReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader, nameof(reader));

            try
            {
                this.Opcode = reader.GetUInt16();
                this.Data = reader.GetBytes((int)reader.Remaining);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
