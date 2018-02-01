using System.IO;

namespace Poirot
{
    internal class CharValue : PrimitiveValue
    {
        readonly char value;
        public CharValue(char value)
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