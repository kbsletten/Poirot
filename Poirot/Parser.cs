using System;
using System.Collections.Generic;
using System.Linq;

namespace Poirot
{
    internal static class Parser
    {
        internal static List<ITemplate> GetTemplates(string template)
        {
            return GetTemplates(template, Lexer.GetTokens(template).GetEnumerator());
        }
        private static List<ITemplate> GetTemplates(string template, IEnumerator<Token> tokens, string context = null)
        {
            var templates = new List<ITemplate>();
            while (tokens.MoveNext())
            {
                var token = tokens.Current;
                if (token.Type == TagType.Text)
                {
                    templates.Add(new LiteralTemplate(template, token.Offset, token.Length));
                    continue;
                }
                var text = template.Substring(token.Offset, token.Length);
                switch (token.Type)
                {
                    case TagType.Escaped:
                        templates.Add(new EscapeTemplate(text));
                        break;
                    case TagType.Attribute:
                        templates.Add(new AttributeTemplate(text));
                        break;
                    case TagType.Partial:
                        templates.Add(new PartialTemplate(text));
                        break;
                    case TagType.Loop:
                        templates.Add(new SectionTemplate(text, GetTemplates(template, tokens, text)));
                        break;
                    case TagType.Inverted:
                        templates.Add(new InvertedTemplate(text, GetTemplates(template, tokens, text)));
                        break;
                    case TagType.End:
                        if (context != text)
                            throw new Exception("Unmatched end tag. Got " + text + " expecting " + (context ?? "nothing"));
                        return templates;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return templates;
        }
    }
}