using System.IO;

namespace Poirot
{
    internal class UintValue : PrimitiveValue
    {
        readonly uint value;
        public UintValue(uint value)
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