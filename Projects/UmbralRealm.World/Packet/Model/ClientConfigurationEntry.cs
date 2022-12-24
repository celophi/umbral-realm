using BinarySerialization;
using UmbralRealm.Core.IO;

namespace UmbralRealm.World.Packet.Model
{
    /// <summary>
    /// Entry in the client.ini file.
    /// </summary>
    public class ClientConfigurationEntry
    {
        /// <summary>
        /// The index associated with a certain line in the file.
        /// </summary>
        [FieldOrder(0)]
        public ClientConfigurationType Type { get; set; }

        /// <summary>
        /// The file associated with a certain line in the file.
        /// </summary>
        [FieldOrder(1)]
        public LengthPrefixedString Value { get; set; } = new();
    }
}
