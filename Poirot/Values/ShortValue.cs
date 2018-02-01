using System.IO;

namespace Poirot
{
    internal class ShortValue : PrimitiveValue
    {
        readonly short value;
        public ShortValue(short value)
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