namespace UmbralRealm.Login
{
    /// <summary>
    /// Result of a login attempt that controls what message is displayed on the client.
    /// </summary>
    public enum LoginFailureResult : ushort
    {
        /// <summary>
        /// Displays the message "Can't connect to the server".
        /// </summary>
        ConnectionFailure1 = 0,

        /// <summary>
        /// Displays the message "Can't connect to the server".
        /// </summary>
        ConnectionFailure2 = 1,

        /// <summary>
        /// Displays the message "Invalid username or password. Password must contain at least one uppercase letter"
        /// </summary>
        InvalidCredentials1 = 2,

        /// <summary>
        /// Displays the message "Invalid username or password. Password must contain at least one uppercase letter"
        /// </summary>
        InvalidCredentials2 = 3,

        /// <summary>
        /// Displays the message "System error".
        /// </summary>
        SystemError = 4,

        /// <summary>
        /// Displays the message "This account has been suspended"
        /// </summary>
        AccountSuspended = 5,

        /// <summary>
        /// Displays the message "Server down"
        /// </summary>
        ServerDown = 6,

        /// <summary>
        /// Displays the message "This game account has been locked. Please contact the staff about this"
        /// </summary>
        AccountLocked = 7,
    }
}
