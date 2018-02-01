using System.IO;

namespace Poirot
{
    internal class LongValue : PrimitiveValue
    {
        readonly long value;
        public LongValue(long value)
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