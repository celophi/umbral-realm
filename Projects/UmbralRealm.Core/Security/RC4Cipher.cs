using UmbralRealm.Core.Security.Interfaces;

namespace UmbralRealm.Core.Security
{
    /// <summary>
    /// Cipher class used for providing basic security for network communication.
    /// </summary>
    public class RC4Cipher : ICipher
    {
        /// <summary>
        /// Schedule that will be used for encryption and decryption.
        /// </summary>
        private readonly int[] _schedule;

        /// <summary>
        /// Used for mixing the schedule for subsequent runs.
        /// </summary>
        private int _indexA = 0;

        /// <summary>
        /// Used for mixing the schedule for subsequent runs.
        /// </summary>
        private int _indexB = 0;

        /// <summary>
        /// Secret key that was used for generating the schedule.
        /// </summary>
        public readonly byte[] Key;

        /// <summary>
        /// Constructs the network cipher using RSA parameters as input.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="schedule"></param>
        private RC4Cipher(int[] schedule, byte[] key)
        {
            _schedule = schedule;
            this.Key = key;
        }

        /// <summary>
        /// Returns the size of the schedule, temporary vectors, and cyclic limit, for all routines.
        /// </summary>
        private static int BlockSize => byte.MaxValue + 1;

        /// <summary>
        /// Initializes an RC4 schedule given a secret key.
        /// </summary>
        /// <param name="key">Byte array used to seed the schedule.</param>
        /// <exception cref="ArgumentException"></exception>
        public static RC4Cipher Create(byte[] key)
        {
            if (key.Length == 0 || key.Length > (BlockSize - 1))
            {
                throw new ArgumentException("The specified key has an invalid or unsupported length", nameof(key));
            }

            // Create a temporary vector that will be used to permute the schedule.
            var schedule = new int[BlockSize];
            var temporary = new int[BlockSize];

            // Fill the state vector with values 0 to 255.
            // Fill the temporary vector with the bytes of the key repeating as necessary.
            for (var index = 0; index < BlockSize; index++)
            {
                schedule[index] = index;
                temporary[index] = key[index % key.Length];
            }

            // Use the temporary vector to produce the initial state vector permutation.
            var indexB = 0;
            for (var indexA = 0; indexA < BlockSize; indexA++)
            {
                // Increment j by the sum of both vectors indexed, but keep it within 0 to 255.
                indexB = (indexB + schedule[indexA] + temporary[indexA]) % BlockSize;

                // Swap values of state[i] and state[j]
                (schedule[indexB], schedule[indexA]) = (schedule[indexA], schedule[indexB]);
            }

            return new RC4Cipher(schedule, key);
        }

        /// <inheritdoc/>
        public byte[] RunCipher(byte[] data)
        {
            lock (_schedule)
            {
                var result = new byte[data.Length];

                for (var iteration = 0; iteration < data.Length; iteration++)
                {
                    _indexA = (_indexA + 1) % BlockSize;
                    _indexB = (_indexB + _schedule[_indexA]) % BlockSize;

                    (_schedule[_indexA], _schedule[_indexB]) = (_schedule[_indexB], _schedule[_indexA]);

                    var value = _schedule[(_schedule[_indexA] + _schedule[_indexB]) % BlockSize];
                    result[iteration] = Convert.ToByte(data[iteration] ^ value);
                }

                return result;
            }
        }
    }
}
