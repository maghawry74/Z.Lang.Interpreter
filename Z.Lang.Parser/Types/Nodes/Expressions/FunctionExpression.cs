using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Exception;
using Z.Lang.Parser.Types.Nodes.Base;
using Z.Lang.Parser.Types.Nodes.Statements;

namespace Z.Lang.Parser.Types.Nodes.Expressions;

public class FunctionExpression(Token token, List<IdentifierExpression> parameters, BlockStatement body) : Expression(token)
{
    public List<IdentifierExpression> Parameters { get; set; } = parameters;
    public BlockStatement Body { get; set; } = body;
    public override string ToString()
    {
        return $"fn({string.Join(", ", Parameters)}) => {Body}";
    }

    public override object Evaluate(Environment? environment = null) => this;
}