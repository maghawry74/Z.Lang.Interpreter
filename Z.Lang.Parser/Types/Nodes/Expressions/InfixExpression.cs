using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class InfixExpression : Expression
{
    private static readonly List<string> Operators = ["+", "-", "*", "/", ">", "<", ">=", "<=", "==", "!="];
    private Expression Left { get; set; }
    private string Operator { get; }
    public Expression Right { get; set; }

    public InfixExpression(Token token, Expression left, string @operator, Expression right) : base(token)
    {
        Left = left;
        Operator = Operators.Contains(@operator)
            ? @operator
            : throw new InvalidOperationException($"{@operator} is not supported");
        Right = right;
    }

    public override string ToString()
    {
        return $"({Left} {Operator} {Right})";
    }

    public override object Evaluate(Environment? environment = null)
    {
        var left = Left.Evaluate(environment);
        var right = Right.Evaluate(environment);

        if (left?.GetType() != right?.GetType())
            throw new InvalidOperationException(
                $"Type mismatch. Left: {left?.GetType().Name} Right: {right?.GetType().Name}");
        
        if (Operator is "==" or "!=") return HandleEquals(left, right);
        
        if (left is string s1 && right is string s2 && Operator is "+") return HandleStringConcat(s1, s2);
        
        return HandleMath((long?)left, (long?)right);
    }

    private static object HandleStringConcat(string left, string right)
    {
        return left + right;
    }

    private object HandleMath(long? left, long? right)
    {
        if (left is null || right is null)
            throw new InvalidOperationException($"Cannot perform {left} {Operator} {right} because of null");
        return Operator switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => left / right,
            ">" => left > right,
            "<" => left < right,
            ">=" => left >= right,
            "<=" => left <= right,
            _ => throw new InvalidOperationException(Operator)
        };
    }

    private bool HandleEquals(object? left, object? right)
    {
        return Operator switch
        {
            "==" => left?.ToString() == right?.ToString(),
            "!=" => left?.ToString() != right?.ToString(),
            _ => throw new InvalidOperationException(Operator)
        };
    }
}