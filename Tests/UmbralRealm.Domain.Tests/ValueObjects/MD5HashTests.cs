using System;
using System.Threading.Tasks;
using UmbralRealm.Domain.ValueObjects;
using UmbralRealm.Login.Data;
using Xunit;

namespace UmbralRealm.Domain.Tests.ValueObjects
{
    public class MD5HashTests
    {
        [Fact]
        public async Task Failure()
        {
            var a = new DbConnectionFactory("Data Source=localhost\\SQLEXPRESS;Initial Catalog=UmbralRealm;Integrated Security=SSPI;User Id=UmbralRealmReadWrite;Password=UmbralRealmReadWrite");
            var p = new AccountRepository(a);

            var aa = await p.GetByUsername(new Username("admin2"));
        }

        [Fact]
        public void WhenConstructed_WithANullArgument_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new MD5Hash(null!));
        }

        [Fact]
        public void WhenConstructed_WithAStringOfInvalidLength_ThrowsException()
        {
            var value = new string('a', MD5Hash.HashLength + 1);
            Assert.Throws<ArgumentException>(() => new MD5Hash(value));
        }

        [Fact]
        public void WhenConstructed_WithNotHexadecimalCharacters_ThrowsException()
        {
            var value = new string('X', MD5Hash.HashLength);
            Assert.Throws<ArgumentException>(() => new MD5Hash(value));
        }

        [Fact]
        public void WhenHashesHaveSameValue_AreCompared_TheyAreEqual()
        {
            var value = "098f6bcd4621d373cade4e832627b4f6";
            var hashA = new MD5Hash(value);
            var hashB = new MD5Hash(value);

            Assert.Equal(hashA, hashB);
        }
    }
}
