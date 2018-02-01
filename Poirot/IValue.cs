using System.Collections.Generic;
using System.IO;

namespace Poirot
{
    public interface IValue : IEnumerable<IValue>
    {
        bool IsTruthy { get; }
        bool IsIterable { get; }
        IValue this[string property] { get; }
        void Render(TextWriter writer, bool htmlEscape);
    }
}