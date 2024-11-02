using Z.Lang.Lexer.Types;

namespace Z.Lang.Parser.Exception;

public class UnExpectedTokenException : System.Exception
{
    public UnExpectedTokenException(TokenType expected, TokenType actual) : base($"Parser error: Expected {expected.ToString()} but got {actual.ToString()}!") { } 
    public UnExpectedTokenException(string message) : base(message) { }
}