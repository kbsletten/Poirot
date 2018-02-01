using System.IO;

namespace Poirot
{
    internal class UShortValue : PrimitiveValue
    {
        readonly ushort value;
        public UShortValue(ushort value)
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