using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Types.Nodes.Base;
using Z.Lang.Parser.Types.Nodes.Statements;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class IfExpression(Token token, Expression condition, BlockStatement then, BlockStatement? @else) : Expression(token)
{
    private Expression Condition { get; set; } = condition;
    private BlockStatement Then { get; set; } = then;
    private BlockStatement? Else { get; set; } = @else;
    public override string ToString() => $"{Token.Literal} {Condition} {Then} {Else}";
    public override object?Evaluate(Environment? environment = null) => Condition.Evaluate(environment) is bool and true ? Then.Evaluate(environment) : Else?.Evaluate(environment);
}