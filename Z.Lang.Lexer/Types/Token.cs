namespace Z.Lang.Lexer.Types;

public class Token
{
    public Token(char literal, TokenType type)
    {
        Literal = literal.ToString();
        Type = type;
    }

    public Token(string literal, TokenType type)
    {
        Literal = literal;
        Type = type;
    }

    public string Literal { get; set; }
    public TokenType Type { get; set; }
}

public enum TokenType
{
    Illegal,
    Eof,

    Identifier,
    Number,
    String,
    Null,
    
    Assign,
    Plus,
    Minus,
    Bang,
    Asterisk,
    Slash,
    Lt,
    Gt,
    Eq,
    NotEq,
    Lte,
    Gte,
    Comma,
    Semicolon,
    Colon,

    LParen,
    RParen,
    LBrace,
    RBrace,
    LSquare,
    RSquare,
    
    Function,
    Let,
    True,
    False,
    If,
    Else,
    Return,
}