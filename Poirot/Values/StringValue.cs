using System.IO;
using System.Net;

namespace Poirot
{
    internal class StringValue : PrimitiveValue
    {
        readonly string value;
        public StringValue(string value)
        {
            this.value = value;
        }
        public override bool IsTruthy => value?.Length > 0;
        public override void Render(TextWriter writer, bool htmlEscape)
        {
            writer.Write(htmlEscape ? WebUtility.HtmlEncode(value) : value);
        }
    }
}