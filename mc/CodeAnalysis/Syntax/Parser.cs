namespace Magick.CodeAnalysis.Syntax
{
    //
    //
    // + 1
    // -1 * -3
    // -(3 + 3)
    //
    //
    //    -
    //    |
    //    *
    //   / \
    //  1   2

    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokens;
        private int _position;
        private List<string> _diagnostics = new();

        public IEnumerable<string> Diagnostics => _diagnostics;

        private SyntaxToken Current => Peek(0);

        public Parser(string text)
        {
            List<SyntaxToken> tokens = new();
            Lexer lexer = new Lexer(text);

            SyntaxToken token;
            do
            {
                token = lexer.Lex();

                if (token.Kind != SyntaxKind.WhitespaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }
            } while (token.Kind != SyntaxKind.EndOfFileToken);

            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        private SyntaxToken Peek(int offset)
        {
            int index = _position + offset;
            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];

            return _tokens[index];
        }

        private SyntaxToken NextToken()
        {
            SyntaxToken current = Current;
            _position++;
            return current;
        }

        private SyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}>, expected <{kind}>");
            return new SyntaxToken(kind, Current.Position, null, null);
        }

        public SyntaxTree Parse()
        {
            ExpressionSyntax expression = ParseExpression();
            SyntaxToken endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }

        private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;
            var unaryOperatorPrecedence = SyntaxFacts.GetUnaryOperatorPrecedence(Current.Kind);
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                SyntaxToken operatorToken = NextToken();
                ExpressionSyntax operand = ParseExpression(unaryOperatorPrecedence);
                left = new UnaryExpressionSyntax(operatorToken, operand);
            }
            else
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                int precendence = SyntaxFacts.GetBinaryOperatorPrecedence(Current.Kind);
                if (precendence == 0 || precendence <= parentPrecedence)
                    break;

                var operatorToken = NextToken();
                var right = ParseExpression(precendence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;

        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.Kind == SyntaxKind.OpenParenthesisToken)
            {
                SyntaxToken left = NextToken();
                ExpressionSyntax expression = ParseExpression();
                SyntaxToken right = MatchToken(SyntaxKind.CloseParenthesisToken);
                return new ParenthesizedExpressionSyntax(left, expression, right);
            }

            SyntaxToken numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
    }
}