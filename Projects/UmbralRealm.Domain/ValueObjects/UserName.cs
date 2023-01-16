using System;
using System.Text.RegularExpressions;

namespace UmbralRealm.Domain.ValueObjects
{
    /// <summary>
    /// Name for an account.
    /// </summary>
    public sealed record Username
    {
        /// <summary>
        /// Maximum length allowed.
        /// </summary>
        public const int MaxLength = 50;

        /// <summary>
        /// Underlying value.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Creates a user name adhering to specific rules.
        /// </summary>
        /// <param name="value"></param>
        public Username(string value)
        {
            if (!IsValid(value))
            {
                throw new ArgumentException("Error. Argument does not meet the validation requirements.", nameof(value));
            }

            this.Value = value;
        }

        /// <summary>
        /// Validates a string value of a user name and returns 'true' if it meets the requirements.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (value.Length > MaxLength)
            {
                return false;
            }

            if (!Regex.IsMatch(value, "^[a-zA-Z0-9]*$", RegexOptions.Compiled))
            {
                return false;
            }

            return true;
        }
    }
}
