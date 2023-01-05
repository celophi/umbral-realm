namespace UmbralRealm.Login.Dto
{
    /// <summary>
    /// Transfer object for the dbo.account table.
    /// </summary>
    public class AccountDto
    {
        /// <summary>
        /// Uniquely identifies the account.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// User's name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// User's password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Personal identification number.
        /// </summary>
        public string Pin { get; set; } = string.Empty;
    }
}
