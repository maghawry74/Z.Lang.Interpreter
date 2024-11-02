using Z.Lang.Lexer.Types;

namespace Z.Lang.Parser.Types.Nodes.Base;

public abstract class Node
{
   public abstract string TokenLiteral();
   public abstract object? Evaluate(Environment? environment = null);
}