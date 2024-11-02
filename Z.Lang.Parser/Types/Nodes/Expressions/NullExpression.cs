using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class NullExpression(Token token) : Expression(token)
{
    private object? Value { get; set; } = null;
    public override string ToString() => $"{Token.Literal}";
    public override object? Evaluate(Environment? environment = null) => Value;
}