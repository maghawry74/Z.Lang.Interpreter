using Z.Lang.Lexer.Types;

namespace Z.Lang.Lexer;

public class Lexer(string input) : ILexer
{
    private int _position;
    private int _readPosition;

    public IEnumerable<Token> GenerateTokens()
    {
        while (TryReadChar(out var ch))
        {
            if (char.IsWhiteSpace(ch)) continue;
            yield return ch switch
            {
                '+' => new Token(ch, TokenType.Plus),

                '=' => PeakChar() == '='
                    ? new Token($"{ch}{(TryReadChar(out var c) ? c.ToString() : string.Empty)}", TokenType.Eq)
                    : new Token(ch, TokenType.Assign),

                '-' => new Token(ch, TokenType.Minus),
                '*' => new Token(ch, TokenType.Asterisk),
                '/' => new Token(ch, TokenType.Slash),

                '<' => PeakChar() == '='
                    ? new Token($"{ch}{(TryReadChar(out var c) ? c.ToString() : string.Empty)}", TokenType.LTE)
                    : new Token(ch, TokenType.Lt),

                '>' => PeakChar() == '='
                    ? new Token($"{ch}{(TryReadChar(out var c) ? c.ToString() : string.Empty)}", TokenType.GTE)
                    : new Token(ch, TokenType.Gt),

                '!' => PeakChar() == '='
                    ? new Token($"{ch}{(TryReadChar(out var c) ? c.ToString() : string.Empty)}", TokenType.NotEq)
                    : new Token(ch, TokenType.Bang),

                '"' => ReadString(), 
                ',' => new Token(ch, TokenType.Comma),
                ';' => new Token(ch, TokenType.Semicolon),
                '(' => new Token(ch, TokenType.LParen),
                ')' => new Token(ch, TokenType.RParen),
                '{' => new Token(ch, TokenType.LBrace),
                '}' => new Token(ch, TokenType.RBrace),
                char.MinValue => new Token(ch, TokenType.Eof),
                _ => Match(ch)
            };
        }
    }

    private  Token ReadString()
    {
        var start = _position + 1;
        while (TryReadChar(out var ch) && ch != '"') { }

        return new Token(input[start.._readPosition], TokenType.String);
    }
    

    private Token Match(char ch)
    {
        if (IsLetter(ch))
        {
            var identifier = ReadIdentifier();
            return new Token(identifier, LookUpIdentifier(identifier));
        }

        return IsNumber(ch) ? new Token(ReadDigits(), TokenType.Int) : new Token(string.Empty, TokenType.Illegal);
    }

    private bool TryReadChar(out char ch)
    {
        ch = char.MinValue;
        if (_readPosition >= input.Length) return false;
        _position = _readPosition;
        _readPosition++;
        ch = input[_position];
        return true;
    }

    private char PeakChar()
    {
        return _readPosition >= input.Length ? char.MinValue : input[_readPosition];
    }

    private string ReadIdentifier()
    {
        var start = _position;
        while (IsLetter(PeakChar()))
        {
            TryReadChar(out _);
        }

        return input[start.._readPosition];
    }

    private string ReadDigits()
    {
        var start = _position;
        // Todo Handle floating point
        while (IsNumber(PeakChar()))
        {
            TryReadChar(out _);
        }

        return input[start.._readPosition];
    }

    private static bool IsLetter(char ch) => ch is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_';
    private static bool IsNumber(char ch) => ch is >= '0' and <= '9';

    private static TokenType LookUpIdentifier(string word)
    {
        return word switch
        {
            "let" => TokenType.Let,
            "fn" => TokenType.Function,
            "true" => TokenType.True,
            "false" => TokenType.False,
            "if" => TokenType.If,
            "else" => TokenType.Else,
            "return" => TokenType.Return,
            _ => TokenType.Identifier,
        };
    }
}