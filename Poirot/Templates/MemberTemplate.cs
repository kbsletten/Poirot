using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Poirot
{
    internal abstract class MemberTemplate : ITemplate
    {
        readonly string[] _name;
        public MemberTemplate(string name)
        {
            _name = name == "." ? new[] { name } : name.Split('.');
        }
        public IValue Get(IEnumerable<IValue> values)
        {
            foreach (var value in values)
            {
                var val = value;
                for (int i = 0; i < _name.Length; i++)
                {
                    val = val?[_name[i]];
                }
                if (val != null)
                {
                    return val;
                }
            }
            return null;
        }
        void ITemplate.Render(TextWriter writer, IReadOnlyDictionary<string, string> partials, Stack<IValue> values)
        {
            Render(writer, partials, values);
        }
        protected abstract void Render(TextWriter writer, IReadOnlyDictionary<string, string> partials, Stack<IValue> values);
    }
}