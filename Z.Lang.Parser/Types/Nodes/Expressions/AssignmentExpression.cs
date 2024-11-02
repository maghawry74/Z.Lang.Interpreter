using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Exception;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class AssignmentExpression(Token token, Expression left, Expression value) : Expression(token)
{
    public override object? Evaluate(Environment? environment = null)
    {
        if (environment is null) throw new NoEnvironmentException();
        var value1 = value.Evaluate(environment);
        environment.ReAssign(left.Token.Literal, value1);
        return value1;
    }
}