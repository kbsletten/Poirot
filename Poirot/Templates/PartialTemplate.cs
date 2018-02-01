using System.Collections.Generic;
using System.IO;

namespace Poirot
{
    internal class PartialTemplate : ITemplate
    {
        readonly string _name;
        public PartialTemplate(string name)
        {
            _name = name;
        }
        void ITemplate.Render(TextWriter writer, IReadOnlyDictionary<string, string> partials, Stack<IValue> values)
        {
            if (partials.TryGetValue(_name, out var partial))
            {
                var templates = Parser.GetTemplates(partial);
                foreach (var template in templates)
                {
                    template.Render(writer, partials, values);
                }
            }
        }
    }
}