using System.Text.RegularExpressions;
using Z.Lang.Lexer.Types;

namespace Z.Lang.Lexer;

public class Lexer(string input) : ILexer
{
    private int _position;
    private int _readPosition;

    public Token NextToken()
    {
        while (TryReadChar(out var ch))
        {
            if (char.IsWhiteSpace(ch)) continue;
             return ch switch
            {
                '+' => new Token(ch, TokenType.Plus),

                '=' => PeakChar() == '='
                    ? new Token($"{ch}{(TryReadChar(out var c) ? c.ToString() : string.Empty)}", TokenType.Eq)
                    : new Token(ch, TokenType.Assign),

                '-' => new Token(ch, TokenType.Minus),
                '*' => new Token(ch, TokenType.Asterisk),
                '/' => new Token(ch, TokenType.Slash),

                '<' => PeakChar() == '='
                    ? new Token($"{ch}{(TryReadChar(out var c) ? c.ToString() : string.Empty)}", TokenType.Lte)
                    : new Token(ch, TokenType.Lt),

                '>' => PeakChar() == '='
                    ? new Token($"{ch}{(TryReadChar(out var c) ? c.ToString() : string.Empty)}", TokenType.Gte)
                    : new Token(ch, TokenType.Gt),

                '!' => PeakChar() == '='
                    ? new Token($"{ch}{(TryReadChar(out var c) ? c.ToString() : string.Empty)}", TokenType.NotEq)
                    : new Token(ch, TokenType.Bang),

                ',' => new Token(ch, TokenType.Comma),
                ';' => new Token(ch, TokenType.Semicolon),
                ':' => new Token(ch, TokenType.Colon),
                '(' => new Token(ch, TokenType.LParen),
                ')' => new Token(ch, TokenType.RParen),
                '{' => new Token(ch, TokenType.LBrace),
                '}' => new Token(ch, TokenType.RBrace),
                '[' => new Token(ch, TokenType.LSquare),
                ']' => new Token(ch, TokenType.RSquare),
                char.MinValue => new Token(nameof(TokenType.Eof), TokenType.Eof),
                _ => Match()
            };
        }
        return new Token(nameof(TokenType.Eof), TokenType.Eof);
    }

    private Token Match()
    {
       var pattern =string.Join("|",
        // Keywords
        @"(?<Keyword>\b(if|else|let|fn|true|false|return)\b)",
        
        // Numbers (including decimals, scientific notation, and hex)
        @"(?<Number>\b\d*\.?\d+([eE][-+]?\d+)?\b|\b0x[0-9a-fA-F]+\b)",
        
        // Identifiers
        @"(?<Identifier>[a-zA-Z_][a-zA-Z_]*)",
        
        // Strings
        """(?<String>"([^"\\]|\\.)*")"""
    );
      
        var regex = new Regex(pattern);
        var match = regex.Match(input, _position);
        
        if (!match.Success) return new Token(string.Empty, TokenType.Illegal);

        _readPosition = match.Index + match.Length;
           
        var value = match.Value.StartsWith("\"") ? match.Value.Substring(1, match.Value.Length - 2) : match.Value;
        
        return new Token(value, LookUpIdentifier(match.Value));
    }

    private bool TryReadChar(out char ch)
    {
        ch = char.MinValue;
        if (_readPosition > input.Length) return false;
        _position = _readPosition;
        ch = _readPosition == input.Length ? ch : input[_position];
        _readPosition++;
        return true;
    }

    private char PeakChar()
    {
        return _readPosition >= input.Length ? char.MinValue : input[_readPosition];
    }
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
            "null" => TokenType.Null,
            _ when word[0] is >= '0' and <= '9' => TokenType.Number,
            _ when word[0] is '"'  => TokenType.String,
            _ => TokenType.Identifier,
        };
    }
}