using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Poirot
{
    internal class Value
    {
        static volatile ConcurrentDictionary<Type, Lazy<Func<object, IValue>>> constructors;
        internal static IValue ForItem<T>(T value)
        {
            if (constructors == null)
            {
                lock (typeof(Value))
                {
                    if (constructors == null)
                    {
                        var tmp = new ConcurrentDictionary<Type, Lazy<Func<object, IValue>>>();
                        tmp.TryAdd(typeof(bool), new Lazy<Func<object, IValue>>(() => (b => new BoolValue((bool)b))));
                        tmp.TryAdd(typeof(byte), new Lazy<Func<object, IValue>>(() => (b => new ByteValue((byte)b))));
                        tmp.TryAdd(typeof(sbyte), new Lazy<Func<object, IValue>>(() => (b => new SbyteValue((sbyte)b))));
                        tmp.TryAdd(typeof(short), new Lazy<Func<object, IValue>>(() => (s => new ShortValue((short)s))));
                        tmp.TryAdd(typeof(ushort), new Lazy<Func<object, IValue>>(() => (s => new UShortValue((ushort)s))));
                        tmp.TryAdd(typeof(int), new Lazy<Func<object, IValue>>(() => (i => new IntValue((int)i))));
                        tmp.TryAdd(typeof(uint), new Lazy<Func<object, IValue>>(() => (i => new UintValue((uint)i))));
                        tmp.TryAdd(typeof(long), new Lazy<Func<object, IValue>>(() => (l => new LongValue((long)l))));
                        tmp.TryAdd(typeof(ulong), new Lazy<Func<object, IValue>>(() => (l => new UlongValue((ulong)l))));
                        tmp.TryAdd(typeof(float), new Lazy<Func<object, IValue>>(() => (f => new FloatValue((float)f))));
                        tmp.TryAdd(typeof(double), new Lazy<Func<object, IValue>>(() => (d => new DoubleValue((double)d))));
                        tmp.TryAdd(typeof(decimal), new Lazy<Func<object, IValue>>(() => (d => new DecimalValue((decimal)d))));
                        tmp.TryAdd(typeof(char), new Lazy<Func<object, IValue>>(() => (c => new CharValue((char)c))));
                        tmp.TryAdd(typeof(string), new Lazy<Func<object, IValue>>(() => (s => new StringValue(s.ToString()))));
                        constructors = tmp;
                    }
                }
            }
            if (value == null) return NullValue.Instance;
            Lazy<Func<object, IValue>> constructor;
            Type type = value.GetType();
            while (!constructors.TryGetValue(type, out constructor))
            {
                constructor = new Lazy<Func<object, IValue>>(() =>
                {
                    var typeProvider = Activator.CreateInstance(typeof(ValueProvider<>).MakeGenericType(type));
                    var valueType = typeof(Value<>).MakeGenericType(type);
                    return (Func<object, IValue>)(t => (IValue)Activator.CreateInstance(valueType, typeProvider, t));
                });
                if (constructors.TryAdd(type, constructor))
                    break;
            }
            return constructor.Value(value);
        }
        public static IEnumerator<IValue> FromEnumerable<T>(IEnumerable<T> enumerable) => enumerable.Select(ForItem).GetEnumerator();
    }
    internal class Value<T> : IValue
    {
        private readonly ValueProvider<T> _provider;
        private readonly T _value;
        public Value(ValueProvider<T> provider, T value)
        {
            _provider = provider;
            _value = value;
        }
        public bool IsTruthy => _provider.IsTruthyFunc(_value);
        public bool IsIterable => _provider.IsIterableFunc(_value);
        public IValue this[string property] => property == "." ? this : _provider.ItemsFunc(_value, property);
        public void Render(TextWriter writer, bool htmlEscape)
        {
            _provider.RenderAction(_value, writer, htmlEscape);
        }
        public IEnumerator<IValue> GetEnumerator() => _provider.GetEnumeratorFunc(_value);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
