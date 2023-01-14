namespace UmbralRealm.Types.Entities
{
    /// <summary>
    /// Represents a persistent state of an account.
    /// </summary>
    /// <param name="AccountId">Uniquely identifies the account.</param>
    /// <param name="Email">Address of the account.</param>
    /// <param name="Username">Name of the account.</param>
    /// <param name="Password">Password of the account.</param>
    /// <param name="Pin">Pin number for the account.</param>
    public sealed record AccountEntity(
        int? AccountId,
        string Username,
        string Password,
        string? Pin
    );
}
