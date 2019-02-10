using System;
using SharpToml.Model;
using SharpToml.Parsing;
using SharpToml.Syntax;
using SharpToml.Text;

namespace SharpToml
{
    public static class Toml
    {
        public static DocumentSyntax Parse(string text, string sourcePath = null, TomlParserOptions options = TomlParserOptions.ParseAndValidate)
        {
            var textView = new StringSourceView(text, sourcePath ?? string.Empty);
            var lexer = new Lexer<StringSourceView, StringCharacterIterator>(textView, textView.SourcePath);
            var parser = new Parser<StringSourceView>(lexer);
            var doc = parser.Run();
            if (!doc.HasErrors && options == TomlParserOptions.ParseAndValidate)
            {
                Validate(doc);
            }
            return doc;
        }

        public static TomlTable ToModel(this DocumentSyntax syntax)
        {
            if (syntax == null) throw new ArgumentNullException(nameof(syntax));
            return TomlTable.From(syntax);
        }

        public static DocumentSyntax Validate(DocumentSyntax doc)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));
            if (doc.HasErrors) return doc;
            var validator = new SyntaxValidator(doc.Diagnostics);
            validator.Visit(doc);
            return doc;
        }
    }
}
