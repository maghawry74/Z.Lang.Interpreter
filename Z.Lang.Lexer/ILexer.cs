using Z.Lang.Lexer.Types;

namespace Z.Lang.Lexer;

public interface ILexer
{
    IEnumerable<Token> GenerateTokens();
}