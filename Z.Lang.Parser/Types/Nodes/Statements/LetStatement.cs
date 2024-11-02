using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Exception;
using Z.Lang.Parser.Types.Nodes.Base;
using Z.Lang.Parser.Types.Nodes.Expressions;

namespace Z.Lang.Parser.Types.Nodes.Statements;

public class LetStatement(Token token) : Statement(token)
{
    public IdentifierExpression Name { get; set; } = null!;
    public Expression Value { get; set; } = null!;
    public override string ToString() => $"{Token.Literal} {Name} = {Value};";
    public override object? Evaluate(Environment? environment = null)
    {
        if (environment is null) throw new NoEnvironmentException(); 
        var res = Value.Evaluate(environment);
        environment.Set(Name.Value, res);
        return res;
    }
}