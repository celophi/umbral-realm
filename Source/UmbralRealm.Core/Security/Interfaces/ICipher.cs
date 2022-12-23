namespace UmbralRealm.Core.Security.Interfaces
{
    public interface ICipher
    {
        /// <summary>
        /// Encrypts or Decrypts specified data according to the previously initialized schedule.
        /// </summary>
        /// <param name="data">Payload to either encrypt or decrypt.</param>
        /// <returns></returns>
        byte[] RunCipher(byte[] data);
    }
}
