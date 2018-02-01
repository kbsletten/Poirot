using System.IO;

namespace Poirot
{
    internal class SbyteValue : PrimitiveValue
    {
        readonly sbyte value;
        public SbyteValue(sbyte value)
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