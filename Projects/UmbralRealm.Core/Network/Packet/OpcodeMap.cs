using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Core.Network.Packet
{
    /// <summary>
    /// Mapping between packet opcode and model, handler, and metadata.
    /// </summary>
    public class OpcodeMap : IOpcodeMap
    {
        /// <inheritdoc/>
        public ushort Opcode { get; set; }

        /// <inheritdoc/>
        public Type? Model { get; set; }

        /// <inheritdoc/>
        public ServerType ServerType { get; set; }

        /// <inheritdoc/>
        public PacketOrigin Origin { get; set; }
    }
}
