using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace UmbralRealm.Core.Security
{
    /// <summary>
    /// RSA certificate used for authenticating client requests.
    /// </summary>
    public class NetworkCertificate
    {
        /// <summary>
        /// Default key size to use for generated keys.
        /// </summary>
        private const int RSA_KEY_SIZE = 2048;

        /// <summary>
        /// Default exponent for RSA keys generated. Conforms to OpenSSL specifications.
        /// </summary>
        private const ushort RSA_PUBLIC_EXPONENT = 0xFFFF;

        /// <summary>
        /// Default certainty for RSA keys.
        /// </summary>
        private const int RSA_CERTAINTY = 5;

        /// <summary>
        /// Random context needed for generating keys.
        /// </summary>
        private static readonly SecureRandom _random = new();

        /// <summary>
        /// RSA public key.
        /// </summary>
        private readonly RsaKeyParameters _publicKey;

        /// <summary>
        /// RSA engine used for decrypting.
        /// </summary>
        private readonly IAsymmetricBlockCipher _rsaEngine;

        /// <summary>
        /// Constructs the network certificate using RSA parameters as input.
        /// </summary>
        /// <param name="rsaEngine"></param>
        /// <param name="publicKey"></param>
        private NetworkCertificate(IAsymmetricBlockCipher rsaEngine, RsaKeyParameters publicKey)
        {
            _rsaEngine = rsaEngine;
            _publicKey = publicKey;
        }

        /// <summary>
        /// Creates a cipher class used for decrypting network traffic.
        /// </summary>
        /// <returns></returns>
        public static async Task<NetworkCertificate> CreatePrivateAsync()
        {
            var cipherKeyPair = await Task.Run(() =>
            {
                var exponent = new BigInteger(1, BitConverter.GetBytes(RSA_PUBLIC_EXPONENT));
                var parameters = new RsaKeyGenerationParameters(exponent, _random, RSA_KEY_SIZE, RSA_CERTAINTY);
                var generator = new RsaKeyPairGenerator();

                generator.Init(parameters);

                // This operation takes a significant amount of time.
                return generator.GenerateKeyPair();
            });

            var publicKey = (RsaKeyParameters)cipherKeyPair.Public;

            var engine = new Pkcs1Encoding(new RsaEngine());
            engine.Init(forEncryption: false, cipherKeyPair.Private);

            return new NetworkCertificate(engine, publicKey);
        }

        /// <summary>
        /// Creates a cipher class used for encrypting network traffic.
        /// </summary>
        /// <param name="modulus">RSA modulus for a public key</param>
        /// <param name="exponent">RSA exponent for a public key</param>
        /// <returns></returns>
        public static NetworkCertificate CreatePublic(byte[] modulus, byte[] exponent)
        {
            var modulusParameter = new BigInteger(1, modulus);
            var exponentParameter = new BigInteger(1, exponent);

            var publicKey = new RsaKeyParameters(isPrivate: false, modulusParameter, exponentParameter);

            var engine = new Pkcs1Encoding(new RsaEngine());
            engine.Init(forEncryption: true, publicKey);

            return new NetworkCertificate(engine, publicKey);
        }

        /// <summary>
        /// Returns the RSA public key modulus that was initialized.
        /// </summary>
        public byte[] GetRsaModulus()
        {
            // Remove the leading 0x00 byte to get just the modulus without sign.
            var modulus = _publicKey.Modulus.ToByteArray();
            return modulus.SkipWhile(value => value == 0).ToArray();
        }

        /// <summary>
        /// Returns the RSA public key exponent that was initialized.
        /// </summary>
        public byte[] GetRsaExponent()
        {
            // Remove the leading 0x00 byte to get just the modulus without sign.
            var exponent = _publicKey.Exponent.ToByteArray();
            return exponent.SkipWhile(value => value == 0).ToArray();
        }

        /// <summary>
        /// Encrypts or decrypts a block of bytes based on initialized engine settings using the RSA initialized key pair.
        /// </summary>
        /// <param name="data">Data to either encrypt or decrypt.</param>
        /// <returns></returns>
        public byte[] ProcessBlock(byte[] data) => _rsaEngine.ProcessBlock(data, 0, data.Length);
    }
}
