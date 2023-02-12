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
                object operand = EvaluateExpression(u.Operand);

                // perform the apropriate operation on operand
                switch (u.Op.Kind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        return (int)operand;
                    case BoundUnaryOperatorKind.Negation:
                        return -(int)operand;
                    case BoundUnaryOperatorKind.LogicalNegation:
                        return !(bool)operand;
                    default:
                        // if an unexpected operator arises then throw an exception
                        throw new Exception($"Unexpected unary operator {u.Op.Kind}");
                }
            }

            // if the node is a binary expression
            if (node is BoundBinaryExpression b)
            {
                // evaluate it's children to get integer values for left and right
                object left = EvaluateExpression(b.Left);
                object right = EvaluateExpression(b.Right);

                // perform the apropriate operation on left and right based on the operator
                switch (b.Op.Kind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return (int)left + (int)right;
                    case BoundBinaryOperatorKind.Subtraction:
                        return (int)left - (int)right;
                    case BoundBinaryOperatorKind.Multiplication:
                        return (int)left * (int)right;
                    case BoundBinaryOperatorKind.Division:
                        return (int)left / (int)right;
                    case BoundBinaryOperatorKind.LogicalAnd:
                        return (bool)left && (bool)right;
                    case BoundBinaryOperatorKind.LogicalOr:
                        return (bool)left || (bool)right;
                    case BoundBinaryOperatorKind.Equal:
                        return Equals(left, right);
                    case BoundBinaryOperatorKind.NotEqual:
                        return !Equals(left, right);
                    default:
                        // if an unexpected operator arises then throw an exception
                        throw new Exception($"Unexpected binary operator {b.Op}");
                }
            }

            // if the node type was not detected then throw an exception
            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}
