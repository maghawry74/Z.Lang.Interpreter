using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Types.DataTypes;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Statements;

public class BlockStatement(Token token, List<Statement> statements) : Statement(token)
{
    private List<Statement> Statements { get; set; } = statements;
    public override string ToString() => string.Join("\n", Statements);
    public override object?Evaluate(Environment? environment = null)
    {
         environment ??= new Environment();
        var result = default(object);
        foreach (var statement in Statements)
        {
            if(statement is ReturnStatement) return new ReturnValue(statement.Evaluate(environment));
            result = statement.Evaluate(environment);
            if (result is ReturnValue) return result;
        }

        return result;
    }
}