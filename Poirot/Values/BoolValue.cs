using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Poirot
{
    internal class BoolValue : PrimitiveValue
    {
        readonly bool value;
        public BoolValue(bool value)
        {
            this.value = value;
        }
        public override bool IsTruthy => value;
        public override void Render(TextWriter writer, bool htmlEscape)
        {
            writer.Write(value);
        }
    }
}