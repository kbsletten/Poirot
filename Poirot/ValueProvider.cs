using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;

namespace Poirot
{
    internal class ValueProvider<T>
    {
        public Func<T, bool> IsTruthyFunc { get; private set; }
        public Func<T, bool> IsIterableFunc { get; private set; }
        public Func<T, string, IValue> ItemsFunc { get; private set; }
        public Func<T, IEnumerator<IValue>> GetEnumeratorFunc { get; private set; }
        public Action<T, TextWriter, bool> RenderAction { get; private set; }
        public ValueProvider()
        {
            var t = Expression.Parameter(typeof(T), "t");
            if (typeof(T) == typeof(bool))
            {
                IsTruthyFunc = Expression.Lambda<Func<T, bool>>(t, t).Compile();
            }
            else if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                Expression isIt = Expression.Call(Expression.Call(t, typeof(IEnumerable).GetMethod("GetEnumerator", new Type[0]), new Expression[0]), typeof(IEnumerator).GetMethod("MoveNext", new Type[0]), new Expression[0]);
                if (!typeof(T).GetTypeInfo().IsValueType)
                {
                    isIt = Expression.AndAlso(Expression.NotEqual(t, Expression.Constant(null, typeof(T))), isIt);
                }
                IsTruthyFunc =
                    Expression.Lambda<Func<T, bool>>(isIt, t).Compile();
            }
            else
            {
                var isNaN = typeof(T).GetMethod("IsNaN", new[] { typeof(T) });
                if (isNaN != null && isNaN.IsStatic)
                {
                    IsTruthyFunc =
                        Expression.Lambda<Func<T, bool>>(Expression.Call(typeof(T), "IsNaN", new Type[0], t), t)
                            .Compile();
                }
                else if (typeof(T).GetTypeInfo().IsValueType)
                {
                    IsTruthyFunc = t1 => true;
                }
                else
                {
                    IsTruthyFunc = t1 => t1 != null;
                }
            }
            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                IsIterableFunc = IsTruthyFunc;
            }
            else
            {
                IsIterableFunc = t1 => false;
            }
            var dicttype =
                typeof(T).GetInterfaces()
                    .Where(
                        i =>
                            i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>) &&
                            i.GetGenericArguments()[0] == typeof(string))
                    .Select(i => i.GetGenericArguments()[1])
                    .FirstOrDefault();
            var p = Expression.Parameter(typeof(string), "p");
            if (dicttype != null)
            {
                var methodInfo = typeof(T).GetProperty("Item", dicttype, new[] { typeof(string) }).GetGetMethod();
                ItemsFunc =
                    Expression.Lambda<Func<T, string, IValue>>(
                        ToRenderer(dicttype, Expression.Call(t, methodInfo, new[] { (Expression)p })),
                        t,
                        p).Compile();
            }
            else
            {
                var ret = Expression.Label(typeof(IValue));
                var cases = new List<SwitchCase>();
                foreach (var symbol in typeof(T).GetFields().Select(x => x.Name).Concat(typeof(T).GetProperties().Select(x => x.Name)))
                {
                    Console.WriteLine("{0}.{1}", typeof(T).Name, symbol);
                }
                cases.AddRange(typeof(T).GetFields().Where(f => !f.IsStatic).Select(field => Expression.SwitchCase(Expression.Return(ret, ToRenderer(field.FieldType, Expression.Field(t, field))), Expression.Constant(field.Name))));
                cases.AddRange(typeof(T).GetProperties().Where(par => !par.GetGetMethod().IsStatic).Select(property => Expression.SwitchCase(Expression.Return(ret, ToRenderer(property.PropertyType, Expression.Property(t, property))), Expression.Constant(property.Name))));
                cases.AddRange(typeof(T).GetMethods().Where(par => !par.IsStatic && par.GetParameters().Length == 0 && par.ReturnType != typeof(void)).Select(property => Expression.SwitchCase(Expression.Return(ret, ToRenderer(property.ReturnType, Expression.Call(t, property, new Expression[0]))), Expression.Constant(property.Name))));
                if (cases.Count == 0)
                {
                    ItemsFunc = (t1, p1) => null;
                }
                else
                {
                    ItemsFunc = Expression.Lambda<Func<T, string, IValue>>(
                        Expression.Block(typeof(IValue),
                            Expression.Switch(p, cases.ToArray()),
                            Expression.Label(ret, Expression.Constant(null, typeof(IValue)))), t, p).Compile();
                }
            }
            var listtype =
                typeof(T).GetInterfaces()
                    .Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .Select(i => i.GetGenericArguments()[0])
                    .FirstOrDefault();
            if (listtype != null)
            {
                GetEnumeratorFunc =
                    Expression.Lambda<Func<T, IEnumerator<IValue>>>(
                        Expression.Call(typeof(Value), "FromEnumerable",
                            new Type[] { listtype }, t), t).Compile();
            }
            else
            {
                GetEnumeratorFunc = null;
            }
            if (typeof(T).GetTypeInfo().IsValueType)
            {
                RenderAction = (t1, w1, b1) => w1.Write(b1 ? WebUtility.HtmlEncode(t1.ToString()) : t1.ToString());
            }
            else
            {
                RenderAction = (t1, w1, b1) =>
                {
                    if (t1 != null)
                    {
                        w1.Write(b1 ? WebUtility.HtmlEncode(t1.ToString()) : t1.ToString());
                    }
                };
            }
        }
        private static Expression ToRenderer(Type type, Expression value)
        {
            Type valueType;
            if (type == typeof(bool))
            {
                valueType = typeof(BoolValue);
            }
            else if (type == typeof(byte))
            {
                valueType = typeof(ByteValue);
            }
            else if (type == typeof(short))
            {
                valueType = typeof(ShortValue);
            }
            else if (type == typeof(ushort))
            {
                valueType = typeof(UShortValue);
            }
            else if (type == typeof(int))
            {
                valueType = typeof(IntValue);
            }
            else if (type == typeof(uint))
            {
                valueType = typeof(UintValue);
            }
            else if (type == typeof(long))
            {
                valueType = typeof(LongValue);
            }
            else if (type == typeof(ulong))
            {
                valueType = typeof(UlongValue);
            }
            else if (type == typeof(float))
            {
                valueType = typeof(FloatValue);
            }
            else if (type == typeof(double))
            {
                valueType = typeof(DoubleValue);
            }
            else if (type == typeof(decimal))
            {
                valueType = typeof(DecimalValue);
            }
            else if (type == typeof(char))
            {
                valueType = typeof(CharValue);
            }
            else if (type == typeof(string))
            {
                valueType = typeof(StringValue);
            }
            else
            {
                return Expression.Call(typeof(Value), "ForItem", new Type[] { type }, new[] { value });
            }
            return Expression.New(valueType.GetConstructor(new Type[] { type }), value);
        }
    }
}