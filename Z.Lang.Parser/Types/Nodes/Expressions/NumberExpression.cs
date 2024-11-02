using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class NumberExpression(Token token, long value) : Expression(token)
{
    public long Value { get; set; } = value;
    public override string ToString() => $"{Value}";
    public override object Evaluate(Environment? environment = null) => Value;
}