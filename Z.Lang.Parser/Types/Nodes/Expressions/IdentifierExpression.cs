using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Exception;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class IdentifierExpression(Token token) : Expression(token)
{
    public string Value => Token.Literal;

    public override string ToString() => $"{Value}";
    public override object? Evaluate(Environment? environment = null)
    {
        if(environment == null) throw new NoEnvironmentException();
       return environment.Get(Value);
    }
}
