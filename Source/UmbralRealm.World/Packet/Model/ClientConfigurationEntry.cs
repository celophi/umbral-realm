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
        public ClientConfigurationType Type { get; set; }

        /// <summary>
        /// The file associated with a certain line in the file.
        /// </summary>
        public string Value { get; set; } = string.Empty;
    }
}
