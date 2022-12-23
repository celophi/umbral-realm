namespace UmbralRealm.Core.IO.Interfaces
{
    /// <summary>
    /// Contract for objects that can be serialized/deserialized and sent between client and server.
    /// </summary>
    public interface IBinarySerializable
    {
        /// <summary>
        /// Serializes a structure into an array of bytes for transmission.
        /// </summary>
        /// <returns></returns>
        byte[] Serialize();

        /// <summary>
        /// Deserializes bytes from a <see cref="BinaryStreamReader"/> to represent a typed structure.
        /// </summary>
        /// <param name="reader"></param>
        void Deserialize(BinaryStreamReader reader);
    }
}
