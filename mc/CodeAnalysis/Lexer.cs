namespace Magick.CodeAnalysis
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private int _position;
        private List<string> _diagnostics = new();

        public Lexer(string text)
        {
            _text = text;
        }

        public IEnumerable<String> Diagnostics => _diagnostics;

        private char Current
        {
            get{
                if (_position >= _text.Length)
                    return '\0';

                return _text[_position];
            }
        }

        private void Next()
        {
            _position++;
        }

        public SyntaxToken Lex()
        {
            // <numbers>
            // <whitespace>
            // + - * / ( )

            // Check for end of file
            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
            }

            // look for NumberTokens
            if (char.IsDigit(Current))
            {
                // set start position and search for end position
                int start = _position;
                while (char.IsDigit(Current))
                    Next();

                // attempt to parse the found string as an integer
                int length = _position - start;
                string text = _text.Substring(start, length);
                if (!int.TryParse(text, out int value))
                {
                    // Print diagnostic error messate
                    _diagnostics.Add($"The number {_text} isn't a valid int32");
                }

                // convert the found string to a token and return
                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            // search for whitespaceTokens
            if (char.IsWhiteSpace(Current))
            {
                // set start position and search for end position
                int start = _position;
                while (char.IsWhiteSpace(Current))
                    Next();

                // convert the found string to a token and return
                int length = _position - start;
                string text = _text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            // search for single special character tokens
            switch (Current)
            {
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
                case '(':
                    return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
                case ')':
                    return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
            }

            // report any unrecognized characters as bad tokens and create a bad token to return
            _diagnostics.Add($"ERROR: bad character in input: '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}