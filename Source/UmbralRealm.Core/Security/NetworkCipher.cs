using UmbralRealm.Core.Security.Interfaces;

namespace UmbralRealm.Core.Security
{
    /// <summary>
    /// Dual cipher class used for encrypting incoming and outgoing network traffic.
    /// </summary>
    public class NetworkCipher
    {
        /// <summary>
        /// Secret key that was used for generating the schedule.
        /// </summary>
        public readonly byte[] Key;

        /// <summary>
        /// RC4 instance used for outgoing network packets.
        /// </summary>
        public readonly ICipher Sending;

        /// <summary>
        /// RC4 instance used for incoming network packets.
        /// </summary>
        public readonly ICipher Receiving;

        /// <summary>
        /// Creates a dual network cipher based on the RC4 algorithm.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sendingCipher"></param>
        /// <param name="receivingCipher"></param>
        private NetworkCipher(byte[] key, ICipher sendingCipher, ICipher receivingCipher)
        {
            this.Key = key;
            this.Sending = sendingCipher;
            this.Receiving = receivingCipher;
        }

        /// <summary>
        /// Creates a cipher used for encrypting/decrypting packets over a network.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static NetworkCipher Create(byte[] key)
        {
            var sending = RC4Cipher.Create(key);
            var receiving = RC4Cipher.Create(key);
            return new NetworkCipher(key, sending, receiving);
        }
    }
}
