namespace Magik.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts
    {
         public static int GetUnaryOperatorPrecedence(SyntaxKind kind)
        {
            switch(kind)
            {                    
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                    return 6;
                    

                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(SyntaxKind kind)
        {
            switch(kind)
            {
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 5;
                    
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;
                    
                case SyntaxKind.EqualEqualToken:
                case SyntaxKind.BangEqualToken:
                    return 3;
                
                case SyntaxKind.AmpersandAmpersandToken:
                    return 2;

                case SyntaxKind.PipePipeToken:
                    return 1;

                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeywordKind(string text)
        {
            switch (text)
            {
                case "true":
                    return SyntaxKind.TrueKeyword;
                case "false":
                    return SyntaxKind.FalseKeyword;
                default:
                    return SyntaxKind.IdentifierToken;
            }
        }
    }
}
