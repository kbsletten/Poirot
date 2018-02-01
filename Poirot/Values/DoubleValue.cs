using System.IO;

namespace Poirot
{
    internal class DoubleValue : PrimitiveValue
    {
        readonly double value;
        public DoubleValue(double value)
        {
            this.value = value;
        }
        public override bool IsTruthy => value != 0 && !double.IsNaN(value);
        public override void Render(TextWriter writer, bool htmlEscape)
        {
            writer.Write(value);
        }
    }
}