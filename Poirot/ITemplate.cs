using System.Collections.Generic;
using System.IO;

namespace Poirot
{
    internal interface ITemplate
    {
        void Render(TextWriter writer, IReadOnlyDictionary<string, string> partials, Stack<IValue> values);
    }
}