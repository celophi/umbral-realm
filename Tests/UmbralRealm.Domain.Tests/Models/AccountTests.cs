using System;
using UmbralRealm.Domain.Models;
using UmbralRealm.Domain.ValueObjects;
using Xunit;

namespace UmbralRealm.Domain.Tests.Models
{
    public class AccountTests
    {
        [Fact]
        public void ConstructUsingNameAndPassword_NameArgumentIsNull_ThrowsArgumentNullException()
        {
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            Assert.Throws<ArgumentNullException>(() => new Account(name: null!, password: password));
        }

        [Fact]
        public void ConstructUsingNameAndPassword_PasswordArgumentIsNull_ThrowsArgumentNullException()
        {
            var username = new UserName("username");
            Assert.Throws<ArgumentNullException>(() => new Account(name: username, password: null!));
        }

        [Fact]
        public void ConstructUsingNameAndPassword_BothArgumentsAreNotNull_SetsInternalValues()
        {
            var username = new UserName("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);

            Assert.Equal(username, account.Name);
            Assert.Equal(password, account.Password);
        }

        [Fact]
        public void CreatePinNumber_ArgumentIsNull_ThrowsArgumentNullException()
        {
            var username = new UserName("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);

            Assert.Throws<ArgumentNullException>(() => account.CreatePinNumber(newPinNumber: null!));
        }

        [Fact]
        public void CreatePinNumber_PinNumberAlreadyExists_ThrowsInvalidOperationException()
        {
            var username = new UserName("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var pin = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);
            account.CreatePinNumber(pin);

            Assert.Throws<InvalidOperationException>(() => account.CreatePinNumber(pin));
        }

        [Fact]
        public void CreatePinNumber_PinNumberDoesNotExist_SetsPinNumber()
        {
            var username = new UserName("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var pin = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);
            account.CreatePinNumber(pin);

            Assert.Equal(pin, account.Pin);
        }

        [Fact]
        public void UpdatePinNumber_CurrentPinNumberArgumentIsNull_ThrowsArgumentNullException()
        {
            var username = new UserName("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var pin = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);
            account.CreatePinNumber(pin);

            Assert.Throws<ArgumentNullException>(() => account.UpdatePinNumber(currentPinNumber: null!, newPinNumber: pin));
        }

        [Fact]
        public void UpdatePinNumber_NewPinNumberArgumentIsNull_ThrowsArgumentNullException()
        {
            var username = new UserName("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var pin = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);
            account.CreatePinNumber(pin);

            Assert.Throws<ArgumentNullException>(() => account.UpdatePinNumber(currentPinNumber: pin, newPinNumber: null!));
        }

        [Fact]
        public void UpdatePinNumber_CurrentPinNumberDoesNotEqualCurrentPinArgument_ThrowsInvalidOperationException()
        {
            var username = new UserName("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var pin = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);
            account.CreatePinNumber(pin);

            var invalidPin = new MD5Hash("098f6bcd4621d373cade4e832627b4f7");

            Assert.Throws<InvalidOperationException>(() => account.UpdatePinNumber(invalidPin, pin));
        }

        [Fact]
        public void UpdatePinNumber_CurrentPinNumberCorrect_SetsNewPinNumber()
        {
            var username = new UserName("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var currentPin = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);
            account.CreatePinNumber(currentPin);

            var newPin = new MD5Hash("098f6bcd4621d373cade4e832627b4f7");
            account.UpdatePinNumber(currentPin, newPin);

            Assert.Equal(newPin, account.Pin);
        }
    }
}
