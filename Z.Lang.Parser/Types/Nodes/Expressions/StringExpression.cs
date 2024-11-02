using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class StringExpression(Token token) : Expression(token)
{
    private string Value => Token.Literal;
    public override string ToString() => $"{Value}";
    public override object Evaluate(Environment? environment = null) => Value;
}