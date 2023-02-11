using Magik.CodeAnalysis.Syntax;
using Magik.CodeAnalysis.Binding;

namespace Magik.CodeAnalysis
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;

        public Evaluator(BoundExpression root)
        {
            _root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            // if the node is a literal value
            if (node is BoundLiteralExpression n)
            {                
                // return the value as an integer
                return n.Value;
            }

            if (node is BoundUnaryExpression u)
            {
                // evaluate the operand to get an integer
                int operand = (int)EvaluateExpression(u.Operand);

                // perform the apropriate operation on operand
                switch (u.OperatorKind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        return operand;
                    case BoundUnaryOperatorKind.Negation:
                        return -operand;
                    default:
                        // if an unexpected operator arises then throw an exception
                        throw new Exception($"Unexpected unary operator {u.OperatorKind}");
                }
            }

            // if the node is a binary expression
            if (node is BoundBinaryExpression b)
            {
                // evaluate it's children to get integer values for left and right
                int left = (int)EvaluateExpression(b.Left);
                int right = (int)EvaluateExpression(b.Right);

                // perform the apropriate operation on left and right based on the operator
                switch (b.OperatorKind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return left + right;
                    case BoundBinaryOperatorKind.Subtraction:
                        return left - right;
                    case BoundBinaryOperatorKind.Multiplication:
                        return left * right;
                    case BoundBinaryOperatorKind.Division:
                        return left / right;
                    default:
                        // if an unexpected operator arises then throw an exception
                        throw new Exception($"Unexpected binary operator {b.OperatorKind}");
                }
            }

            // if the node type was not detected then throw an exception
            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}