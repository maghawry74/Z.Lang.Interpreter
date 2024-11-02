using Z.Lang.Lexer.Types;

namespace Z.Lang.Parser.Types.Nodes.Base;

public abstract class Statement(Token token) : Node
{
    protected Token Token { get; set; } = token;
    public override string ToString() => $"Type: {Token.Type} Literal: {Token.Literal}";
    public override string TokenLiteral() => Token.Literal;
}