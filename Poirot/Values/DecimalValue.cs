using System.IO;

namespace Poirot
{
    internal class DecimalValue : PrimitiveValue
    {
        readonly decimal value;
        public DecimalValue(decimal value)
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