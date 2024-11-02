
using System.Collections;
using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Exception;
using Z.Lang.Parser.Types.DataTypes;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class IndexExpression(Token token, Expression left, Expression index) : Expression(token)
{
    private Expression Left { get; set; } = left;
    private Expression Index { get; set; } = index;

    public override object? Evaluate(Environment? environment = null)
    {
        if (environment is null) throw new NoEnvironmentException();

        var left = Left.Evaluate(environment) ;
        
        var index = Index.Evaluate(environment);
        if (left is ArrayValue a)
        {
            if (index is not long i)
                throw new UnExpectedTokenException($"Unexpected index type : {index?.GetType().Name}");
            if (i < 0 || i >= a.Elements.Count) throw new IndexOutOfRangeException();
            return a.Elements[(int)i];
        }

        if (left is Dictionary<string, object?> d)
        {
            return d!.GetValueOrDefault(index!.ToString());
        }

        throw new UnExpectedTokenException($"Unexpected left type : {left?.GetType().Name}");
    }
    
}