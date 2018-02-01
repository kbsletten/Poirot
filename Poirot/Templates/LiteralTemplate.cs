using System;
using System.Collections.Generic;
using System.IO;

namespace Poirot
{
    internal class LiteralTemplate : ITemplate
    {
        readonly string _template;
        readonly int _offset;
        readonly int _length;
        public LiteralTemplate(string template, int offset, int length)
        {
            _template = template;
            _offset = offset;
            _length = length;
        }
        void ITemplate.Render(TextWriter writer, IReadOnlyDictionary<string, string> partials, Stack<IValue> values)
        {
            // avoid copy of substring
            for (var i = _offset; i < _offset + _length; i++)
            {
                // this will likely be abysmal on non-buffered writers
                // this could be fixed if TextWriter had a method Write(string, int, int)
                // like it does for Write(char[], int, int)
                writer.Write(_template[i]);
            }
        }
    }
}