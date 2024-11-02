using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Types.DataTypes;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Statements;

public class ExpressionStatement(Token token) : Statement(token)
{
    public Expression Expression { get; set; } = null!;
    public override string ToString() => $"{Expression}";
    public override object? Evaluate(Environment? environment = null)
    {
        var result = Expression.Evaluate(environment);
        return result as ReturnValue ?? result;
    }
}