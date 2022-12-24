using System;
using UmbralRealm.Core.Security;
using Xunit;

namespace UmbralRealm.Core.Tests.Security
{
    public class RC4CipherTests
    {
        /// <summary>
        /// Assert that when the supplied key has an invalid length, an exception is thrown.
        /// </summary>
        [Fact]
        public void Create_InvalidKeyLength_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => RC4Cipher.Create(Array.Empty<byte>()));
            Assert.Throws<ArgumentException>(() => RC4Cipher.Create(new byte[byte.MaxValue + 1]));
        }

        /// <summary>
        /// Assert that running the cipher after schedule initialization encrypts and decrypts data correctly.
        /// </summary>
        [Fact]
        public void RunCipher_ScheduleInitialized_PermutatesCorrectly()
        {
            var key = new byte[] { 0xEE, 0xC6, 0x32, 0x06, 0x8B };
            var cipher = RC4Cipher.Create(key);

            var encrypted = new byte[] { 0xC7, 0x91, 0x34, 0xF5, 0x4C, 0xC3 };
            var expected = new byte[] { 0x03, 0x00, 0x00, 0x00, 0xE0, 0x40 };

            var result = cipher.RunCipher(encrypted);

            Assert.Equal(expected, result);
        }
    }
}
