using System.Threading.Tasks;
using UmbralRealm.Core.Security;
using Xunit;

namespace UmbralRealm.Core.Tests.Security
{
    public class NetworkCertificateTests
    {
        /// <summary>
        /// Assert that the public modulus has the correct length.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetRsaModulus_CertificateCreated_ModulusLengthCorrect()
        {
            var certificate = await NetworkCertificate.CreatePrivateAsync();
            var modulus = certificate.GetRsaModulus();
            Assert.Equal(256, modulus.Length);
        }

        /// <summary>
        /// Assert that the public exponent has the correct length.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetRsaExponent_CertificateCreated_ExponentLengthCorrect()
        {
            var certificate = await NetworkCertificate.CreatePrivateAsync();
            var modulus = certificate.GetRsaExponent();
            Assert.Equal(2, modulus.Length);
        }

        /// <summary>
        /// Assert that data encrypted with a public key can successfully be decrypted by a private key.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreatePublic_CorrectModulusAndExponent_Encrypts()
        {
            var data = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0xFF };

            var privateKey = await NetworkCertificate.CreatePrivateAsync();
            var modulus = privateKey.GetRsaModulus();
            var exponent = privateKey.GetRsaExponent();

            var publicKey = NetworkCertificate.CreatePublic(modulus, exponent);
            var encrypted = publicKey.ProcessBlock(data);
            var decrypted = privateKey.ProcessBlock(encrypted);

            Assert.Equal(data, decrypted);
        }
    }
}
