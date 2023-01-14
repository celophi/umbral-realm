using System;
using System.Text.RegularExpressions;

namespace UmbralRealm.Domain.ValueObjects
{
    public sealed record MD5Hash
    {
        /// <summary>
        /// The length of the hash in a string representation.
        /// </summary>
        public const int HashLength = 32;

        /// <summary>
        /// String representation of the hash.
        /// </summary>
        public readonly string Value;

        public MD5Hash(string value)
        {
            if (!IsValid(value))
            {
                throw new ArgumentException("Error. Argument does not meet the validation requirements.", nameof(value));
            }

            this.Value = value.ToUpper();
        }

        /// <summary>
        /// Validates a string value of an MD5 hash and returns 'true' if it meets the requirements.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (value.Length != HashLength)
            {
                return false;
            }

            if (!Regex.IsMatch(value, $"^[0-9a-fA-F]{{{HashLength}}}$", RegexOptions.Compiled))
            {
                return false;
            }

            return true;
        }
    }
}
