using BinarySerialization;
using UmbralRealm.Core.IO;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Core.Network.Packet.Model.Generic;
using UmbralRealm.Core.Security.Interfaces;
using UmbralRealm.Core.Utilities;

namespace UmbralRealm.Core.Network.Packet
{
    /// <summary>
    /// Used to serialize and deserialize packets by means of a <see cref="OpcodeMapping"/>.
    /// </summary>
    public class PacketConverter : IPacketConverter
    {
        private readonly OpcodeMapping _opcodeMapping;

        /// <summary>
        /// Used for wrapping reflection capabilities.
        /// </summary>
        private readonly ActivatorWrapper _activator;

        public PacketConverter(OpcodeMapping opcodeMapping, ActivatorWrapper activator)
        {
            _opcodeMapping = opcodeMapping ?? throw new ArgumentNullException(nameof(opcodeMapping));
            _activator = activator ?? throw new ArgumentNullException(nameof(activator));
        }

        /// <inheritdoc/>
        public byte[] Serialize(IPacket packet, ICipher cipher)
        {
            ArgumentNullException.ThrowIfNull(packet);
            ArgumentNullException.ThrowIfNull(cipher);

            using var plainTextWriter = new BinaryStreamWriter();
            using var encryptedWriter = new BinaryStreamWriter();

            if (_opcodeMapping.TryGetByModel(packet.GetType(), out var map))
            {
                plainTextWriter.PutUInt16(map.Opcode);
            }
            else if (packet is not UnknownPacket unknownPacket)
            {
                throw new KeyNotFoundException($"Error. Model type {packet.GetType()} has not been registered.");
            }

            using var stream = new MemoryStream();
            var serializer = new BinarySerializer();
            serializer.Serialize(stream, packet);

            plainTextWriter.PutBytes(stream.ToArray());

            var encrypted = cipher.RunCipher(plainTextWriter.ToArray());
            encryptedWriter.PutUInt16((ushort)encrypted.Length);
            encryptedWriter.PutBytes(encrypted);
            return encryptedWriter.ToArray();
        }

        /// <inheritdoc/>
        public IList<IPacket> Deserialize(byte[] buffer, ICipher cipher)
        {
            ArgumentNullException.ThrowIfNull(cipher);

            if (buffer.Length < sizeof(ushort))
            {
                throw new ArgumentException($"Unable to deserialize to packet because the buffer length is too short.", nameof(buffer));
            }

            var packets = new List<IPacket>();
            using var reader = new BinaryStreamReader(buffer);

            while (reader.Remaining > 0)
            {
                var length = reader.GetUInt16();
                var payload = reader.GetBytes(length);
                var decrypted = cipher.RunCipher(payload);
                var packet = this.Deserialize(decrypted);

                packets.Add(packet);
            }

            return packets;
        }

        /// <summary>
        /// Deserializes a buffer into a single packet.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private IPacket Deserialize(byte[] buffer)
        {
            if (buffer.Length < sizeof(ushort))
            {
                throw new ArgumentException($"Unable to deserialize to packet because the buffer length is too short.", nameof(buffer));
            }

            using var reader = new BinaryStreamReader(buffer);
            var opcode = reader.GetUInt16();
            var remaining = reader.GetRemaining();

            _opcodeMapping.TryGetByOpcode(opcode, out var map);
            var packetType = map?.Model ?? typeof(UnknownPacket);

            try
            {
                // TODO: Find a cleaner way to include the opcode either always or never.
                if (packetType != typeof(UnknownPacket))
                {
                    buffer = remaining;
                }

                var serializer = new BinarySerializer();
                return (IPacket)serializer.Deserialize(buffer, packetType);
            }
            catch (Exception ex)
            {
                // TODO: log the entire buffer.
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
