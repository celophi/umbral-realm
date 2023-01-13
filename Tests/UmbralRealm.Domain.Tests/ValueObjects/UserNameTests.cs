using System;
using UmbralRealm.Domain.ValueObjects;
using Xunit;

namespace UmbralRealm.Domain.Tests.ValueObjects
{
    public class UserNameTests
    {
        [Fact]
        public void Construct_ValueIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new UserName(null!));
        }

        [Fact]
        public void Construct_ValueLengthIsTooSmall_ThrowsArgumentOutOfRangeException()
        {
            var value = new string('x', UserName.MinLength - 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => new UserName(value));
        }

        [Fact]
        public void Construct_ValueLengthIsTooBig_ThrowsArgumentOutOfRangeException()
        {
            var value = new string('x', UserName.MaxLength + 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => new UserName(value));
        }

        [Fact]
        public void Construct_ValueHasInvalidCharacters_ThrowsFormatException()
        {
            var value = "!@#$%^&*()_+=-:;{}[]|";
            Assert.Throws<FormatException>(() => new UserName(value));
        }

        [Fact]
        public void Construct_ValueHasValidCharacters_SetsInternalValue()
        {
            var value = "MyName";
            var username = new UserName(value);

            Assert.Equal(value, username.Value);
        }
    }
}
