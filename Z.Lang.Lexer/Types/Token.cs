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
    Int,
    String,
    
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
    LTE,
    GTE,
    Comma,
    Semicolon,

    LParen,
    RParen,
    LBrace,
    RBrace,

    Function,
    Let,
    True,
    False,
    If,
    Else,
    Return,
}