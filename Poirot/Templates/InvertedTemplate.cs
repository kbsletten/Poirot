using System.Collections.Generic;
using System.IO;

namespace Poirot
{
    internal class InvertedTemplate : MemberTemplate
    {
        readonly List<ITemplate> _inner;
        public InvertedTemplate(string name, List<ITemplate> inner)
            : base(name)
        {
            _inner = inner;
        }
        protected override void Render(TextWriter writer, IReadOnlyDictionary<string, string> partials, Stack<IValue> values)
        {
            var val = Get(values);
            if (val != null && val.IsTruthy)
                return;
            for (var i = 0; i < _inner.Count; i++)
            {
                _inner[i].Render(writer, partials, values);
            }
        }
    }
}