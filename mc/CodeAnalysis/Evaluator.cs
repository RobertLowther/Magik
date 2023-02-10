namespace Magick.CodeAnalysis
{
    public sealed class Evaluator
    {
        private readonly ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax root)
        {
            _root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax node)
        {
            // if the node is a literal value
            if (node is LiteralExpressionSyntax n)
            {
                // return -1 if node value is null
                if (n.LiteralToken.Value == null)
                    return -1;
                
                // return the value as an integer
                return (int) n.LiteralToken.Value;
            }

            // if the node is a binary expression
            if (node is BinaryExpressionSyntax b)
            {
                // evaluate it's children to get integer values for left and right
                int left = EvaluateExpression(b.Left);
                int right = EvaluateExpression(b.Right);

                // perform the arpropriate operation an left and rightt based on the operator
                if (b.OperatorToken.Kind == SyntaxKind.PlusToken)
                    return left + right;
                if (b.OperatorToken.Kind == SyntaxKind.MinusToken)
                    return left - right;
                if (b.OperatorToken.Kind == SyntaxKind.StarToken)
                    return left * right;
                if (b.OperatorToken.Kind == SyntaxKind.SlashToken)
                    return left / right;
                else
                    // if an unexpected operator arises then throw an exception
                    throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}");
            }

            // if the node is a parenthesis node then evaluate the expression it contains
            // and return an integer value
            if (node is ParenthesizedExpressionSyntax p)
                return EvaluateExpression(p.Expression);

            // if the node type was not detected then throw an exception
            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}