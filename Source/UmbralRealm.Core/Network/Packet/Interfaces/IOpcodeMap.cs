namespace UmbralRealm.Core.Network.Packet.Interfaces
{
    /// <summary>
    /// Contract for declaring a mapping between packet opcode and model, handler, and metadata.
    /// </summary>
    public interface IOpcodeMap
    {
        /// <summary>
        /// Uniquely identifies the associated model and functionality of a packet message.
        /// </summary>
        public ushort Opcode { get; }

        /// <summary>
        /// Packet model to associate with the <see cref="Opcode"/>.
        /// </summary>
        public Type? Model { get; }

        /// <summary>
        /// Indicates what type of server this mapping should be a part of.
        /// </summary>
        public ServerType ServerType { get; }

        /// <summary>
        /// Indicates which communicator should be the source of this packet information.
        /// </summary>
        public PacketOrigin Origin { get; }
    }
}
