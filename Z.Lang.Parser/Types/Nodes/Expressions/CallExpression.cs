using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Exception;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class CallExpression(Token token, List<Expression> arguments, Expression function) : Expression(token)
{
    public List<Expression> Arguments { get; set; } = arguments;
    public Expression Function { get; set; } = function;
    public override string ToString() => $"{Function}({string.Join(", ", Arguments)})";

    public override object? Evaluate(Environment? environment = null)
    {
        if (environment is null) throw new NoEnvironmentException();
        var functionName = ((IdentifierExpression)Function).Value;
        var fn = environment.Get(functionName)!;
        
        if(fn is Delegate d) return d.DynamicInvoke(Arguments.Select(x => x.Evaluate(environment)).ToArray());
        
        var functionExpression = (FunctionExpression)fn;
        if (Arguments.Count != functionExpression.Parameters.Count) throw new InvalidOperationException($"Arguments mismatch : expected {functionExpression.Parameters.Count} but got {Arguments.Count}");
        var functionEnv = new Environment(environment);
        for (var i = 0; i < Arguments.Count; i++)
        {
            functionEnv.Set(functionExpression.Parameters[i].Value, Arguments[i].Evaluate(functionEnv));
        }
       
        return functionExpression.Body.Evaluate(functionEnv);
    }
}