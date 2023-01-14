namespace UmbralRealm.Domain.Enumerations
{
    public enum LoginResult
    {
        /// <summary>
        /// User is not found.
        /// </summary>
        NotFound = 0,

        /// <summary>
        /// User login is accepted.
        /// </summary>
        Authorized = 1,

        /// <summary>
        /// credentials are invalid.
        /// </summary>
        InvalidPassword = 2,
    }
}
