using System;
using UmbralRealm.Domain.Models;
using UmbralRealm.Domain.ValueObjects;
using UmbralRealm.Types.Entities;
using Xunit;

namespace UmbralRealm.Domain.Tests.Models
{
    public class AccountTests
    {
        [Fact]
        public void ConstructUsingNameAndPassword_NameArgumentIsNull_ThrowsArgumentNullException()
        {
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            Assert.Throws<ArgumentNullException>(() => new Account(username: null!, password: password));
        }

        [Fact]
        public void ConstructUsingNameAndPassword_PasswordArgumentIsNull_ThrowsArgumentNullException()
        {
            var username = new Username("username");
            Assert.Throws<ArgumentNullException>(() => new Account(username: username, password: null!));
        }

        [Fact]
        public void ConstructUsingNameAndPassword_BothArgumentsAreNotNull_SetsInternalValues()
        {
            var username = new Username("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);

            Assert.Equal(username, account.Username);
            Assert.Equal(password, account.Password);
        }

        [Fact]
        public void CreatePinNumber_ArgumentIsNull_ThrowsArgumentNullException()
        {
            var username = new Username("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);

            Assert.Throws<ArgumentNullException>(() => account.CreatePinNumber(newPinNumber: null!));
        }

        [Fact]
        public void CreatePinNumber_PinNumberAlreadyExists_ThrowsInvalidOperationException()
        {
            var username = new Username("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var pin = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);
            account.CreatePinNumber(pin);

            Assert.Throws<InvalidOperationException>(() => account.CreatePinNumber(pin));
        }

        [Fact]
        public void CreatePinNumber_PinNumberDoesNotExist_SetsPinNumber()
        {
            var username = new Username("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var pin = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);
            account.CreatePinNumber(pin);

            Assert.Equal(pin, account.Pin);
        }

        [Fact]
        public void UpdatePinNumber_CurrentPinNumberArgumentIsNull_ThrowsArgumentNullException()
        {
            var username = new Username("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var pin = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);
            account.CreatePinNumber(pin);

            Assert.Throws<ArgumentNullException>(() => account.UpdatePinNumber(currentPinNumber: null!, newPinNumber: pin));
        }

        [Fact]
        public void UpdatePinNumber_NewPinNumberArgumentIsNull_ThrowsArgumentNullException()
        {
            var username = new Username("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var pin = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);
            account.CreatePinNumber(pin);

            Assert.Throws<ArgumentNullException>(() => account.UpdatePinNumber(currentPinNumber: pin, newPinNumber: null!));
        }

        [Fact]
        public void UpdatePinNumber_CurrentPinNumberDoesNotEqualCurrentPinArgument_ThrowsInvalidOperationException()
        {
            var username = new Username("username");
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
            var username = new Username("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var currentPin = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);
            account.CreatePinNumber(currentPin);

            var newPin = new MD5Hash("098f6bcd4621d373cade4e832627b4f7");
            account.UpdatePinNumber(currentPin, newPin);

            Assert.Equal(newPin, account.Pin);
        }

        [Fact]
        public void ToEntity_Invoked_ReturnsCurrentState()
        {
            var username = new Username("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var currentPin = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var account = new Account(username, password);
            account.CreatePinNumber(currentPin);

            var entity = account.ToEntity();

            Assert.Null(entity.AccountId);
            Assert.Equal(username.Value, entity.Username);
            Assert.Equal(password.Value, entity.Password);
            Assert.Equal(currentPin.Value, entity.Pin);
        }

        [Fact]
        public void ToEntity_InvokedAndHasAccountId_ReturnsCurrentState()
        {
            var accountId = 10;
            var username = new Username("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");

            var entity = new AccountEntity(accountId, username.Value, password.Value, Pin: null);
            var account = new Account(entity);

            var output = account.ToEntity();

            Assert.Equal(accountId, output.AccountId);
            Assert.Equal(username.Value, output.Username);
            Assert.Equal(password.Value, output.Password);
        }

        [Fact]
        public void ConstructUsingEntity_EntityArgumentIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Account(entity: null!));
        }

        [Fact]
        public void ConstructUsingEntity_EntityHasPin_PinIsSet()
        {
            var accountId = 10;
            var username = new Username("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");
            var pin = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");

            var entity = new AccountEntity(accountId, username.Value, password.Value, pin.Value);
            var account = new Account(entity);

            Assert.Equal(username, account.Username);
            Assert.Equal(password, account.Password);
            Assert.Equal(pin, account.Pin);
        }

        [Fact]
        public void ConstructUsingEntity_EntityDoesNotHavePin_PinIsNotSet()
        {
            var accountId = 10;
            var username = new Username("username");
            var password = new MD5Hash("098f6bcd4621d373cade4e832627b4f6");

            var entity = new AccountEntity(accountId, username.Value, password.Value, Pin: null);
            var account = new Account(entity);

            Assert.Equal(username, account.Username);
            Assert.Equal(password, account.Password);
            Assert.Null(account.Pin);
        }
    }
}
