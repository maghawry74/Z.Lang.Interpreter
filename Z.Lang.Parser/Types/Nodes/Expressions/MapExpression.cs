using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Exception;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class MapExpression(Token token,List<KeyValuePair<Expression,Expression>> map) : Expression(token)
{
    private Dictionary<string, Expression> _map = map.ToDictionary(x=>x.Key.Evaluate()!.ToString()!,x=>x.Value);
    public override object Evaluate(Environment? environment = null)
    {
        if (environment is null) throw new NoEnvironmentException();
        var map = new Dictionary<string, object?>();
        foreach (var (key, value) in _map)
        {
            map.Add(key, value.Evaluate(environment));
        }
        return map;
    }
}