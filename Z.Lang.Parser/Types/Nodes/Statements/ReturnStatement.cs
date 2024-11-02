using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Types.Nodes.Base;

namespace Z.Lang.Parser.Types.Nodes.Statements;

public class ReturnStatement(Token token) : Statement(token)
{
    public Expression ReturnValue { get; set; } = null!;

    public override string ToString() => $"{Token.Literal} {ReturnValue};";
    public override object? Evaluate(Environment? environment = null) => ReturnValue.Evaluate(environment);
}