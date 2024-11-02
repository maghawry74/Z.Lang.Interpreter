using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class GroupedExpression(Token token, Expression expression) : Expression(token)
{
    public Expression Expression { get; set; } = expression;
    public override string ToString() => $"({Expression})";
    public override object? Evaluate(Environment? environment = null) => Expression.Evaluate(environment);
}