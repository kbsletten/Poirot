using System.IO;

namespace Poirot
{
    internal class ByteValue : PrimitiveValue
    {
        readonly byte value;
        public ByteValue(byte value)
        {
            this.value = value;
        }
        public override bool IsTruthy => value != 0;
        public override void Render(TextWriter writer, bool htmlEscape)
        {
            writer.Write(value);
        }
    }
}