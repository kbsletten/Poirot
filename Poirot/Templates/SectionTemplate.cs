using System.Collections.Generic;
using System.IO;

namespace Poirot
{
    internal class SectionTemplate : MemberTemplate
    {
        readonly List<ITemplate> _inner;
        public SectionTemplate(string name, List<ITemplate> inner)
            : base(name)
        {
            _inner = inner;
        }
        protected override void Render(TextWriter writer, IReadOnlyDictionary<string, string> partials, Stack<IValue> values)
        {
            var val = Get(values);
            if (val == null)
                return;
            if (val.IsIterable)
            {
                foreach (var v in val)
                {
                    values.Push(v);
                    foreach (var template in _inner)
                    {
                        template.Render(writer, partials, values);
                    }
                    values.Pop();
                }
            }
            else if (val.IsTruthy)
            {
                values.Push(val);
                foreach (var template in _inner)
                {
                    template.Render(writer, partials, values);
                }
                values.Pop();
            }
        }
    }
}