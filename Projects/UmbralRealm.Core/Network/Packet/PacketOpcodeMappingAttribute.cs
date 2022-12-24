namespace UmbralRealm.Core.Network.Packet
{
    /// <summary>
    /// Decorator for objects or functions that should be mapped to a specific packet opcode.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PacketOpcodeMappingAttribute : Attribute
    {
        /// <summary>
        /// Unique identifier for the packet.
        /// </summary>
        public readonly ushort Opcode;

        public PacketOpcodeMappingAttribute(ushort opcode)
        {
            this.Opcode = opcode;
        }
    }
}
