using System.Collections.Generic;
using System.IO;

namespace Poirot
{
    internal class EscapeTemplate : MemberTemplate
    {
        public EscapeTemplate(string name)
            : base(name)
        {
        }
        protected override void Render(TextWriter writer, IReadOnlyDictionary<string, string> partials, Stack<IValue> values)
        {
            var renderer = Get(values);
            if (renderer != null)
            {
                renderer.Render(writer, false);
            }
        }
    }
}