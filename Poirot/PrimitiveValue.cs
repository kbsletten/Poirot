using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Poirot
{
    internal abstract class PrimitiveValue : IValue
    {

        public IValue this[string property] => this;
        public abstract bool IsTruthy { get; }
        public abstract void Render(TextWriter writer, bool htmlEscape);
        public bool IsIterable => false;
        public IEnumerator<IValue> GetEnumerator() => EmptyEnumerator.Instance;
        IEnumerator IEnumerable.GetEnumerator() => EmptyEnumerator.Instance;
    }
}