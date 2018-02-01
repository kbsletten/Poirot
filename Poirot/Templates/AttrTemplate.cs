using System.Collections.Generic;
using System.IO;

namespace Poirot
{
    internal class AttributeTemplate : MemberTemplate
    {
        public AttributeTemplate(string name)
            : base(name)
        {
        }
        protected override void Render(TextWriter writer, IReadOnlyDictionary<string, string> partials, Stack<IValue> values)
        {
            var value = Get(values);
            if (value != null)
            {
                value.Render(writer, true);
            }
        }
    }
}