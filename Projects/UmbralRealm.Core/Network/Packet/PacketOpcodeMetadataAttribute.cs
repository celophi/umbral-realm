namespace UmbralRealm.Core.Network.Packet
{
    /// <summary>
    /// Decorator for packet opcode enumeration members.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class PacketOpcodeMetadataAttribute : Attribute
    {
        /// <summary>
        /// Represents the sender or creator of the packet.
        /// </summary>
        public readonly PacketOrigin Origin;

        /// <summary>
        /// Represents the type of server and the kind of use cases it servers.
        /// </summary>
        public readonly ServerType ServerType;

        public PacketOpcodeMetadataAttribute(PacketOrigin origin, ServerType serverType)
        {
            this.Origin = origin;
            this.ServerType = serverType;
        }
    }
}
