using System.IO;

namespace Poirot
{
    internal class IntValue : PrimitiveValue
    {
        readonly int value;
        public IntValue(int value)
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