using System.IO;

namespace Poirot
{
    internal class UlongValue : PrimitiveValue
    {
        readonly ulong value;
        public UlongValue(ulong value)
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