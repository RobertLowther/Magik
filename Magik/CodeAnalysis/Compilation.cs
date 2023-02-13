using Magik.CodeAnalysis.Syntax;
using Magik.CodeAnalysis.Binding;

namespace Magik.CodeAnalysis
{
    public sealed class Compilation
    {
        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }

        public SyntaxTree Syntax { get; }

        public EvaluationResult Evaluate()
        {
            var binder = new Binder();
            var BoundExpression = binder.BindExpression(Syntax.Root);

            var diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToArray();
            if (diagnostics.Any())
            {
                return new EvaluationResult(diagnostics, null);
            }

            var evaluator = new Evaluator(BoundExpression);
            var value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<string>(), value);
        }
    }
}
