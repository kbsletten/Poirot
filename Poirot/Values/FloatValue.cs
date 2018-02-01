using System.IO;

namespace Poirot
{
    internal class FloatValue : PrimitiveValue
    {
        readonly float value;
        public FloatValue(float value)
        {
            this.value = value;
        }
        public override bool IsTruthy => value != 0 && !float.IsNaN(value);
        public override void Render(TextWriter writer, bool htmlEscape)
        {
            writer.Write(value);
        }
    }
}