﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Poirot
{
    public static class Mustache
    {
        static readonly IReadOnlyDictionary<string, string> Empty = new Dictionary<string, string>();
        public static string Render<T>(string template, IReadOnlyDictionary<string, string> partials, T value)
        {
            return RenderValue(template, partials, Value.ForItem(value));
        }
        public static string Render<T>(string template, T value)
        {
            return RenderValue(template, Value.ForItem(value));
        }
        public static void Render<T>(string template, IReadOnlyDictionary<string, string> partials, T value, TextWriter writer)
        {
            RenderValue(template, partials, Value.ForItem(value), writer);
        }
        public static void Render<T>(string template, T value, TextWriter writer)
        {
            RenderValue(template, Value.ForItem(value), writer);
        }
        public static string RenderValue(string template, IValue value)
        {
            return RenderValue(template, Empty, value);
        }
        public static string RenderValue(string template, IReadOnlyDictionary<string, string> partials, IValue value)
        {
            var writer = new StringWriter();
            RenderValue(template, partials, value, writer);
            return writer.ToString();
        }
        public static void RenderValue(string template, IValue value, TextWriter writer)
        {
            RenderValue(template, Empty, value, writer);
        }
        public static void RenderValue(string template, IReadOnlyDictionary<string, string> partials, IValue value, TextWriter writer)
        {
            var templates = Parser.GetTemplates(template);
            var values = new Stack<IValue>();
            values.Push(value);
            for (var i = 0; i < templates.Count; i++)
            {
                templates[i].Render(writer, partials, values);
            }
        }
    }
}
