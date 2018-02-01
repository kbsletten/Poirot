using System;
using System.Collections;
using System.Collections.Generic;

namespace Poirot
{
    internal class EmptyEnumerator : IEnumerator<IValue>, IEnumerator
    {
        public static IEnumerator<IValue> Instance = new EmptyEnumerator();
        IValue IEnumerator<IValue>.Current => throw new NotImplementedException();
        object IEnumerator.Current => throw new NotImplementedException();
        void IDisposable.Dispose()
        {
        }
        bool IEnumerator.MoveNext() => false;
        void IEnumerator.Reset()
        {
        }
    }
}