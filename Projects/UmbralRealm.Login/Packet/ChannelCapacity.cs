namespace UmbralRealm.Login.Packet
{
    /// <summary>
    /// Indicates the current channel population as a estimated description.
    /// </summary>
    public enum ChannelCapacity : ushort
    {
        /// <summary>
        /// Channel population is low.
        /// </summary>
        Smooth = 0,

        /// <summary>
        /// Channel population is medium.
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Channel population is high.
        /// </summary>
        Crowded = 2,
    }
}
