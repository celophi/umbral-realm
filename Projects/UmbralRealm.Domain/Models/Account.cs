using System;
using UmbralRealm.Domain.ValueObjects;
using UmbralRealm.Types.Entities;

namespace UmbralRealm.Domain.Models
{
    public sealed class Account
    {
        /// <summary>
        /// Uniquely identifies an account.
        /// </summary>
        private readonly int? AccountId;

        /// <summary>
        /// User name of the account.
        /// </summary>
        public readonly Username Username;

        /// <summary>
        /// Used to authenticate the account.
        /// </summary>
        public readonly MD5Hash Password;

        /// <summary>
        /// Additional verification for authenticating.
        /// </summary>
        public MD5Hash? Pin { get; private set; }

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public Account(Username username, MD5Hash password)
        {
            ArgumentNullException.ThrowIfNull(username);
            ArgumentNullException.ThrowIfNull(password);

            this.Username = username;
            this.Password = password;
        }

        /// <summary>
        /// Creates a PIN number if one does not yet exist.
        /// </summary>
        /// <param name="newPinNumber"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void CreatePinNumber(MD5Hash newPinNumber)
        {
            ArgumentNullException.ThrowIfNull(newPinNumber);

            if (this.Pin != null)
            {
                throw new InvalidOperationException("Error. A PIN has already been created.");
            }

            this.Pin = newPinNumber;
        }

        /// <summary>
        /// Updates an existing PIN number to a new one.
        /// </summary>
        /// <param name="currentPinNumber"></param>
        /// <param name="newPinNumber"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void UpdatePinNumber(MD5Hash currentPinNumber, MD5Hash newPinNumber)
        {
            ArgumentNullException.ThrowIfNull(currentPinNumber);
            ArgumentNullException.ThrowIfNull(newPinNumber);

            if (this.Pin != currentPinNumber)
            {
                throw new InvalidOperationException("Error. PIN number did not match.");
            }

            this.Pin = newPinNumber;
        }

        #region Persistence

        /// <summary>
        /// Creates an account from a previously stored state.
        /// </summary>
        /// <param name="entity"></param>
        public Account(AccountEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            this.AccountId = entity.AccountId;
            this.Username = new Username(entity.Username);
            this.Password = new MD5Hash(entity.Password);

            if (entity.Pin != null)
            {
                this.Pin = new MD5Hash(entity.Pin);
            }
        }

        /// <summary>
        /// Retrieves the current state as an entity.
        /// </summary>
        /// <returns></returns>
        public AccountEntity ToEntity()
        {
            return new AccountEntity
            (
                this.AccountId,
                this.Username.Value,
                this.Password.Value,
                this.Pin?.Value
            );
        }

        #endregion
    }
}
