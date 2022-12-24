using BinarySerialization;

namespace UmbralRealm.Core.IO
{
    public class LengthPrefixedString
    {
        [FieldOrder(0)]
        public ushort Length { get; set; }

        [FieldOrder(1)]
        [FieldLength(nameof(Length))]
        public string Text { get; set; } = string.Empty;
    }
}
