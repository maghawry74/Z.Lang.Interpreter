using Z.Lang.Lexer.Types;

namespace Z.Lang.Parser.Types.Nodes.Base;

public abstract class Expression(Token token) : Node
{
    public Token Token { get; set; } = token;

    public override string ToString() => $"Type: {Token.Type} Literal: {Token.Literal}";
    
    public override string TokenLiteral() => Token.Literal;
}