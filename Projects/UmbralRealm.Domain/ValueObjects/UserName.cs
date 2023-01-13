using System;
using System.Text.RegularExpressions;

namespace UmbralRealm.Domain.ValueObjects
{
    /// <summary>
    /// Name for an account.
    /// </summary>
    public sealed record UserName
    {
        /// <summary>
        /// Maximum length allowed.
        /// </summary>
        public const int MaxLength = 50;

        /// <summary>
        /// Minimum length allowed.
        /// </summary>
        public const int MinLength = 1;

        /// <summary>
        /// Underlying value.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Creates a user name adhering to specific rules.
        /// </summary>
        /// <param name="value"></param>
        public UserName(string value)
        {
            this.Validate(value);
            this.Value = value;
        }

        /// <summary>
        /// Validates a string value of a user name.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        private void Validate(string value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (value.Length < MinLength || value.Length > MaxLength)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (!Regex.IsMatch(value, "^[a-zA-Z0-9]*$"))
            {
                throw new FormatException("Error. The value does not meet the format expected for a user name.");
            }
        }
    }
}
