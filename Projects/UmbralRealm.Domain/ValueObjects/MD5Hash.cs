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
            this.Validate(value);
            this.Value = value.ToUpper();
        }

        private void Validate(string value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (value.Length != HashLength)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (!Regex.IsMatch(value, $"^[0-9a-fA-F]{{{HashLength}}}$", RegexOptions.Compiled))
            {
                throw new FormatException("The value is not a valid MD5 hash.");
            }
        }
    }
}
