using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Exception;
using Z.Lang.Parser.Types.DataTypes;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class ArrayExpression(Token token, List<Expression> elements) : Expression(token)
{
    private List<Expression> Elements { get; set; } = elements;

    public override object Evaluate(Environment? environment = null)
    {
        if (environment is null) throw new NoEnvironmentException();
        return new ArrayValue(Elements.Select(x => x.Evaluate(environment)).ToList());
    }
}