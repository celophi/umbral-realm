using System.Text;

namespace UmbralRealm.Core.IO
{
    /// <summary>
    /// Utility for writing a byte array and writing primitives.
    /// </summary>
    public sealed class BinaryStreamWriter : IDisposable
    {
        private readonly MemoryStream _stream;

        private readonly byte[] _buffer = new byte[16];

        /// <summary>
        /// Returns the length of the underlying stream.
        /// </summary>
        public long Length => _stream.Length;

        /// <summary>
        /// Gets or sets the position of the underlying stream.
        /// </summary>
        public long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }

        public BinaryStreamWriter()
        {
            _stream = new MemoryStream();
        }

        /// <summary>
        /// Flushes all written data and returns a byte array of the stream.
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray() => _stream.ToArray();

        /// <summary>
        /// Writes a byte to the current stream at the current position.
        /// </summary>
        /// <param name="value"></param>
        public void PutByte(byte value)
        {
            _stream.WriteByte(value);
        }

        /// <summary>
        /// Writes a byte array to the stream.
        /// </summary>
        /// <param name="buffer"></param>
        public void PutBytes(byte[] buffer)
        {
            this.PutBytes(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Writes a byte array to the stream.
        /// </summary>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The maximum number of bytes to write.</param>
        public void PutBytes(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        /// <summary>
        /// Writes an array of empty bytes with length specified.
        /// </summary>
        /// <param name="count"></param>
        public void PutEmptyBytes(int count)
        {
            var buffer = new byte[count];
            this.PutBytes(buffer);
        }

        /// <summary>
        /// Writes an unsigned 16-bit integer into the stream.
        /// </summary>
        /// <param name="value"></param>
        public void PutUInt16(ushort value)
        {
            for (var i = 0; i < sizeof(ushort); i++)
            {
                _buffer[i] = (byte)(value >> 8 * i);
            }

            _stream.Write(_buffer, 0, sizeof(ushort));
        }

        /// <summary>
        /// Writes an unsigned 32-bit integer into the stream.
        /// </summary>
        /// <param name="value"></param>
        public void PutUInt32(uint value)
        {
            for (var i = 0; i < sizeof(uint); i++)
            {
                _buffer[i] = (byte)(value >> 8 * i);
            }

            _stream.Write(_buffer, 0, sizeof(uint));
        }

        /// <summary>
        /// Writes an unsigned 64-bit integer into the stream.
        /// </summary>
        /// <param name="value"></param>
        public void PutUInt64(ulong value)
        {
            for (var i = 0; i < sizeof(ulong); i++)
            {
                _buffer[i] = (byte)(value >> 8 * i);
            }

            _stream.Write(_buffer, 0, sizeof(ulong));
        }

        /// <summary>
        /// Writes a signed 16-bit integer into the stream.
        /// </summary>
        /// <param name="value"></param>
        public void PutInt16(short value) => this.PutUInt16((ushort)value);

        /// <summary>
        /// Writes an signed 32-bit integer into the stream.
        /// </summary>
        /// <param name="value"></param>
        public void PutInt32(int value) => this.PutUInt32((uint)value);

        /// <summary>
        /// Writes a signed 64-bit integer into the stream.
        /// </summary>
        /// <param name="value"></param>
        public void PutInt64(long value) => this.PutUInt64((ulong)value);

        /// <summary>
        /// Writes a length-prefixed string where the length is a 16-bit integer.
        /// </summary>
        /// <param name="value"></param>
        public void PutLPString(string value)
        {
            var length = value.Length;
            this.PutUInt16((ushort)length);

            var data = Encoding.UTF8.GetBytes(value);
            this.PutBytes(data);
        }

        /// <summary>
        /// Writes a float to the stream.
        /// </summary>
        /// <param name="value"></param>
        public void PutFloat(float value)
        {
            var buffer = BitConverter.GetBytes(value);
            this.PutBytes(buffer);
        }

        /// <summary>
        /// Disposes the underlying <see cref="MemoryStream"/>
        /// </summary>
        public void Dispose()
        {
            using (_stream) { }
        }
    }
}
