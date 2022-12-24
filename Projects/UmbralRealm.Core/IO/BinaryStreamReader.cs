using System.Text;

namespace UmbralRealm.Core.IO
{
    /// <summary>
    /// Utility for reading a byte array and retrieving primitives.
    /// </summary>
    public sealed class BinaryStreamReader : IDisposable
    {
        public readonly MemoryStream Stream;

        /// <summary>
        /// Gets or sets the position of the underlying stream.
        /// </summary>
        public long Position
        {
            get { return this.Stream.Position; }
            set { this.Stream.Position = value; }
        }

        /// <summary>
        /// Returns the underlying stream length.
        /// </summary>
        public long Length => this.Stream.Length;

        /// <summary>
        /// Returns the number of remaining bytes that can be read.
        /// </summary>
        public long Remaining => this.Stream.Length - this.Stream.Position;

        public BinaryStreamReader(byte[] buffer)
        {
            this.Stream = new MemoryStream(buffer, writable: false);
        }

        /// <summary>
        /// Reads an array of bytes from the stream.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public byte[] GetBytes(int count)
        {
            var buffer = new byte[count];
            if (count != this.Stream.Read(buffer, 0, count))
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return buffer;
        }

        /// <summary>
        /// Gets a single byte from the stream.
        /// </summary>
        /// <returns></returns>
        public byte GetByte() => this.GetBytes(sizeof(byte))[0];

        /// <summary>
        /// Gets an unsigned 16-bit integer from the stream.
        /// </summary>
        /// <returns></returns>
        public ushort GetUInt16()
        {
            var buffer = this.GetBytes(sizeof(ushort));
            return (ushort)(buffer[0] | buffer[1] << 8);
        }

        /// <summary>
        /// Gets an unsigned 32-bit integer from the stream.
        /// </summary>
        /// <returns></returns>
        public uint GetUInt32()
        {
            var buffer = this.GetBytes(sizeof(uint));
            return (uint)(buffer[0] | buffer[1] << 8 | buffer[2] << 16 | buffer[3] << 24);
        }

        /// <summary>
        /// Gets an unsigned 64-bit integer from the stream.
        /// </summary>
        /// <returns></returns>
        public ulong GetUInt64()
        {
            var lower = this.GetUInt32();
            var upper = this.GetUInt32();
            return lower | ((ulong)upper << 32);
        }

        /// <summary>
        /// Gets a 16-bit integer from the stream.
        /// </summary>
        /// <returns></returns>
        public short GetInt16() => (short)this.GetUInt16();

        /// <summary>
        /// Gets a 32-bit integer from the stream.
        /// </summary>
        /// <returns></returns>
        public int GetInt32() => (int)this.GetUInt32();

        /// <summary>
        /// Reads a length-prefixed string where the length is a 16-bit integer.
        /// </summary>
        /// <returns></returns>
        public string GetLPString()
        {
            var length = this.GetUInt16();
            var data = this.GetBytes(length);
            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// Reads a float from the stream.
        /// </summary>
        /// <returns></returns>
        public float GetFloat()
        {
            var buffer = this.GetBytes(sizeof(float));
            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Returns the remaining bytes in the stream.
        /// </summary>
        /// <returns></returns>
        public byte[] GetRemaining()
        {
            return this.GetBytes((int)this.Remaining);
        }

        /// <summary>
        /// Disposes the underlying <see cref="MemoryStream"/>
        /// </summary>
        public void Dispose()
        {
            using (this.Stream) { }
        }
    }
}
