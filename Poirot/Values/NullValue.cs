using System.IO;

namespace Poirot
{
    internal class NullValue : PrimitiveValue
    {
        static internal IValue Instance = new NullValue();
        public override bool IsTruthy => false;
        public override void Render(TextWriter writer, bool htmlEscape)
        {
        }
    }
}