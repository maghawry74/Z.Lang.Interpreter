using Z.Lang.Parser.Types.DataTypes;
using Z.Lang.Parser.Types.Nodes.Base;
using Z.Lang.Parser.Types.Nodes.Statements;

namespace Z.Lang.Parser.Types;

public class Program : Node
{
    public List<Statement> Statements { get; set; } = [];
    public override string TokenLiteral() => Statements.FirstOrDefault()?.TokenLiteral() ?? string.Empty;
    public override object?Evaluate(Environment? environment = null)
    {
        environment ??= new Environment();
        var result = default(object);
        foreach (var statement in Statements)
        {
            if(statement is ReturnStatement) return statement.Evaluate();
            result = statement.Evaluate(environment);
            if (result is ReturnValue returnValue) return returnValue.Value;
        }

        return result;
    }
}