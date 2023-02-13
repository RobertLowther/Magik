namespace Magik.CodeAnalysis.Syntax
{
    public sealed class UnaryExpressionSyntax : ExpressionSyntax
    {
        public UnaryExpressionSyntax(SyntaxToken operatorToken, ExpressionSyntax operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }

        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
        public ExpressionSyntax Operand { get; }
        public SyntaxToken OperatorToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OperatorToken;
            yield return Operand;
        }
    }
}
