using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class PrefixExpression : Expression
{
    public PrefixExpression(Token token, Expression operand,string @operator) : base(token)
    {
        if (!Operators.Contains(@operator)) throw new NotSupportedException($"{@operator} is not supported");
        Operand = operand;
        Operator = @operator;
    }

    private static readonly List<string> Operators = ["!", "-"];
    public Expression Operand { get; set; }
    public string Operator { get; set; }
    public override string ToString() => $"{Operator}{Operand}";
    public override object?Evaluate(Environment? environment = null)
    {
        var value = Operand.Evaluate(environment);
        if (value is null) return value;
        if (Operator == "-" && value is not long) throw new InvalidOperationException($"{value} is not a number");
        if (Operator == "!" && value is not bool) throw new InvalidOperationException($"{value} is not a boolean");
        return Operator switch
        {
            "!" => !(bool)value,
            "-" => -(long)value,
            _ => throw new NotSupportedException($"{Operator} is not supported")
        };
    }
}