using Magik.CodeAnalysis;
using Magik.CodeAnalysis.Binding;
using Magik.CodeAnalysis.Syntax;

namespace Magik
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            bool showTree = false;

            while (true)
            {
                // Get User Input
                Console.Write("> ");
                string? line = Console.ReadLine();

                // Check for special cases
                switch (line)
                {
                    case "/showTree" or "/st":
                        showTree = !showTree;
                        Console.WriteLine(showTree ? "Showing parse trees." : "Not showimg parse trees."); 
                        continue;
                    case "/cls" or "/clear":
                        Console.Clear();
                        continue;
                    case "" or "/quit" or "/q" or null:
                        return;
                }

                SyntaxTree syntaxTree = SyntaxTree.Parse(line);
                Compilation compilation = new Compilation(syntaxTree);
                EvaluationResult result = compilation.Evaluate();

                IReadOnlyList<Diagnostic> diagnostics = result.Diagnostics;

                // if the show tree tag is set
                if (showTree)
                {
                    // Print the tree as it was parsed
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ResetColor();
                }

                // If no errors were reported
                if (!diagnostics.Any())
                {
                    Console.WriteLine(result.Value);
                }
                else
                {
                    // Print the erros in red
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    
                    foreach (Diagnostic diagnostic in diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }

                    Console.ResetColor();
                }
            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", string marker = "", bool isLast = true, bool isRoot = true)
        {
            Console.Write(indent);
            Console.Write(marker);

            Console.Write(node.Kind);

            if (node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            if (!isRoot)
                indent += isLast ? "   " : "│  ";

            SyntaxNode? lastChild = node.GetChildren().LastOrDefault();

            foreach (SyntaxNode child in node.GetChildren())
                PrettyPrint(child, indent, child == lastChild ? "└──" : "├──", child == lastChild, false);
        }
    }
}


//Expression: 
//
//  -+6
//
//SyntaxTree:
//
// . = UnaryExpressionSyntax
// : = BinaryExpressionSyntax
//
//   .
//  / \
// -   .
//    / \
//   +   6
//
//
//BoundTree:
//
//  -
//  |
//  +
//  |
//  6


//Expression: 
//
//  3 + 5 * 4
//
//SyntaxTree:
//
// . = UnaryExpressionSyntax
// : = BinaryExpressionSyntax
//
//    :
//   /|\
//  3 + :
//     /|\
//    5 * 4
//
//
//BoundTree:
//
//    +
//   / \
//  3   *
//     / \
//    5   4
