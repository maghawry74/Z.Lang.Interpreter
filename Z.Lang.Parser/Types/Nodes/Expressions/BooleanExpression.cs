using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class BooleanExpression(Token token) : Expression(token)
{
    private bool Value { get; set; } = token.Literal == "true";
    public override string ToString() => $"{Token.Literal}";
    public override object Evaluate(Environment? environment = null) => Value;
}