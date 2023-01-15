using System;
using UmbralRealm.Domain.ValueObjects;
using Xunit;

namespace UmbralRealm.Domain.Tests.ValueObjects
{
    public class UsernameTests
    {
        [Fact]
        public void Construct_ValueIsNull_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Username(null!));
        }

        [Fact]
        public void Construct_ValueLengthIsTooSmall_ThrowsArgumentException()
        {
            var value = string.Empty;
            Assert.Throws<ArgumentException>(() => new Username(value));
        }

        [Fact]
        public void Construct_ValueLengthIsTooBig_ThrowsArgumentException()
        {
            var value = new string('x', Username.MaxLength + 1);
            Assert.Throws<ArgumentException>(() => new Username(value));
        }

        [Fact]
        public void Construct_ValueHasInvalidCharacters_ThrowsArgumentException()
        {
            var value = "!@#$%^&*()_+=-:;{}[]|";
            Assert.Throws<ArgumentException>(() => new Username(value));
        }

        [Fact]
        public void Construct_ValueHasValidCharacters_SetsInternalValue()
        {
            var value = "MyName";
            var username = new Username(value);

            Assert.Equal(value, username.Value);
        }
    }
}
