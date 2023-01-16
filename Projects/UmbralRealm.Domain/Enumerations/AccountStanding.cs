namespace UmbralRealm.Domain.Enumerations
{
    /// <summary>
    /// The level of trust associated with the account based on previous behavior.
    /// </summary>
    public enum AccountStanding : byte
    {
        /// <summary>
        /// Allowed to all services of the game.
        /// </summary>
        Trusted = 0,

        /// <summary>
        /// Prevented from game access temporarily.
        /// </summary>
        Suspended = 1,

        /// <summary>
        /// Indefinitely banned from access to the game.
        /// </summary>
        Locked = 2,
    }
}
