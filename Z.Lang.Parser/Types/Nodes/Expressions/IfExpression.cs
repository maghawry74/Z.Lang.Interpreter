using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Types.Nodes.Base;
using Z.Lang.Parser.Types.Nodes.Statements;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class IfExpression(Token token, Expression condition, BlockStatement then, BlockStatement? @else) : Expression(token)
{
    public Expression Condition { get; set; } = condition;
    public BlockStatement Then { get; set; } = then;
    public BlockStatement? Else { get; set; } = @else;
    public override string ToString() => $"{Token.Literal} {Condition} {Then} {Else}";
    public override object?Evaluate(Environment? environment = null) => Condition.Evaluate(environment) is bool and true ? Then.Evaluate(environment) : Else?.Evaluate(environment);
}