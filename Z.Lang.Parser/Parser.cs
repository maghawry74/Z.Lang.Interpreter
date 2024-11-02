using Z.Lang.Lexer;
using Z.Lang.Lexer.Types;
using Z.Lang.Parser.Exception;
using Z.Lang.Parser.Types;
using Z.Lang.Parser.Types.Nodes.Base;
using Z.Lang.Parser.Types.Nodes.Expressions;
using Z.Lang.Parser.Types.Nodes.Statements;

namespace Z.Lang.Parser;

public static class Precedence
{
    public static readonly int Lowest = 0;
    public static readonly int Assignment = 1;
    public static readonly int Equal = 2;
    public static readonly int LessGreater = 3;
    public static readonly int Sum = 4;
    public static readonly int Product = 5;
    public static readonly int Prefix = 6;
    public static readonly int Call = 7;
    public static readonly int Index = 8;

    public static readonly Dictionary<TokenType, int> Map = new()
    {
        { TokenType.Eq, Equal },
        { TokenType.NotEq, Equal },
        { TokenType.Lt, LessGreater },
        { TokenType.Gt, LessGreater },
        { TokenType.Plus, Sum },
        { TokenType.Minus, Sum },
        { TokenType.Slash, Product },
        { TokenType.Asterisk, Product },
        { TokenType.LParen, Call },
        { TokenType.LSquare, Index },
        { TokenType.Assign, Assignment }
    };
}

public class Parser : IParser
{
    private readonly ILexer _lexer;
    private Token _currentToken = null!;
    private Token _peekToken = null!;

    public Parser(ILexer lexer)
    {
        _lexer = lexer;
        AdvanceTokens();
        AdvanceTokens();
    }

    public Program Parse()
    {
        var program = new Program();

        while (!CurrentTokenIs(TokenType.Eof))
        {
            program.Statements.Add(ParseStatement());
            AdvanceTokens();
        }

        return program;
    }

    private Statement ParseStatement()
    {
        return _currentToken.Type switch
        {
            TokenType.Let => ParseLetStatement(),
            TokenType.Return => ParseReturnStatement(),
            _ => ParseExpressionStatement()
        };
    }

    private LetStatement ParseLetStatement()
    {
        var statement = new LetStatement(_currentToken);

        ExpectPeek(TokenType.Identifier);

        statement.Name = new IdentifierExpression(_currentToken);

        ExpectPeek(TokenType.Assign);

        AdvanceTokens();
        statement.Value = ParseExpression(Precedence.Lowest);

        ExpectPeek(TokenType.Semicolon);

        return statement;
    }

    private ReturnStatement ParseReturnStatement()
    {
        var statement = new ReturnStatement(_currentToken);

        AdvanceTokens();

        statement.ReturnValue = ParseExpression(Precedence.Lowest);

        ExpectPeek(TokenType.Semicolon);

        return statement;
    }

    private ExpressionStatement ParseExpressionStatement()
    {
        var expressionStatement = new ExpressionStatement(_currentToken)
        {
            Expression = ParseExpression(Precedence.Lowest)
        };

        while (PeekTokenIs(TokenType.Semicolon)) AdvanceTokens();

        return expressionStatement;
    }

    private Expression ParseExpression(int precedence)
    {
        var prefix = MapPrefix(_currentToken.Type);
        var left = prefix();
        while (!PeekTokenIs(TokenType.Semicolon) && precedence < PeekPrecedence())
        {
            var infix = MapInfix(_peekToken.Type);
            AdvanceTokens();
            left = infix(left);
        }

        return left;
    }




    private FunctionExpression ParseFunctionLiteral()
    {
        var token = _currentToken;
        ExpectPeek(TokenType.LParen);
        var parameters = ParseFunctionParameters();
        ExpectPeek(TokenType.LBrace);
        var body = ParseBlockStatement();
        return new FunctionExpression(token, parameters, body);
    }


    private List<IdentifierExpression> ParseFunctionParameters()
    {
        var identifiers = new List<IdentifierExpression>();
        if (PeekTokenIs(TokenType.RParen))
        {
            AdvanceTokens();
            return identifiers;
        }

        AdvanceTokens();
        identifiers.Add(new IdentifierExpression(_currentToken));

        while (PeekTokenIs(TokenType.Comma))
        {
            AdvanceTokens();
            AdvanceTokens();
            identifiers.Add(new IdentifierExpression(_currentToken));
        }

        ExpectPeek(TokenType.RParen);

        return identifiers;
    }

    private IfExpression ParseIfExpression()
    {
        var token = _currentToken;
        ExpectPeek(TokenType.LParen);
        AdvanceTokens();
        var condition = ParseExpression(Precedence.Lowest);
        ExpectPeek(TokenType.RParen);

        AdvanceTokens();
        var consequence = ParseBlockStatement();
        BlockStatement? alternative = default;
        if (PeekTokenIs(TokenType.Else))
        {
            AdvanceTokens();
            AdvanceTokens();
            alternative = ParseBlockStatement();
        }

        return new IfExpression(token, condition, consequence, alternative);
    }

    private BlockStatement ParseBlockStatement()
    {
        var token = _currentToken;
        if (!CurrentTokenIs(TokenType.LBrace))
            throw new UnExpectedTokenException(TokenType.RParen, _currentToken.Type);

        AdvanceTokens();
        var statements = new List<Statement>();
        while (!CurrentTokenIs(TokenType.RBrace) && !CurrentTokenIs(TokenType.Eof))
        {
            statements.Add(ParseStatement());
            AdvanceTokens();
        }

        if (!CurrentTokenIs(TokenType.RBrace))
            throw new UnExpectedTokenException(TokenType.RParen, _currentToken.Type);

        return new BlockStatement(token, statements);
    }


    private GroupedExpression ParseGroupedExpression()
    {
        var token = _currentToken;
        AdvanceTokens();
        var expression = ParseExpression(Precedence.Lowest);

        ExpectPeek(TokenType.RParen);

        return new GroupedExpression(token, expression);
    }

    private CallExpression ParseCallExpression(Expression function)
    {
        var token = _currentToken;
        var arguments = ParseExpressionList();
        return new CallExpression(token, arguments, function);

        List<Expression> ParseExpressionList()
        {
            var args = new List<Expression>();
            if (PeekTokenIs(TokenType.RParen))
            {
                AdvanceTokens();
                return args;
            }

            AdvanceTokens();
            args.Add(ParseExpression(Precedence.Lowest));

            while (PeekTokenIs(TokenType.Comma))
            {
                AdvanceTokens();
                AdvanceTokens();
                args.Add(ParseExpression(Precedence.Lowest));
            }

            ExpectPeek(TokenType.RParen);

            return args;
        }
    }

    private Func<Expression> MapPrefix(TokenType tokenType) => tokenType switch
    {
        TokenType.Identifier => ParseIdentifier,
        TokenType.Number => ParseNumberLiteral,
        TokenType.String => ParseStringLiteral,
        TokenType.Null => ParseNullLiteral,
        TokenType.True or TokenType.False => ParseBooleanLiteral,
        TokenType.Bang => ParsePrefixExpression,
        TokenType.Minus => ParsePrefixExpression,
        TokenType.LParen => ParseGroupedExpression,
        TokenType.LSquare=> ParseArrayLiteral,
        TokenType.LBrace=> ParseMapLiteral,
        TokenType.If => ParseIfExpression,
        TokenType.Function => ParseFunctionLiteral,
        TokenType.Illegal => throw new UnExpectedTokenException(
            $"Parsing Error : Illegal character {_currentToken.Literal}!"),
        _ => throw new UnExpectedTokenException(
            $"Parsing Error : Expected an expression but got {_currentToken.Literal}!"),
    };

    private MapExpression ParseMapLiteral()
    {
        var token = _currentToken;
        var pairs = ParseMapPairs();
        return new MapExpression(token, pairs);

        List<KeyValuePair<Expression, Expression>> ParseMapPairs()
        {
            var result = new List<KeyValuePair<Expression, Expression>>();
           AdvanceTokens();
           if (PeekTokenIs(TokenType.RBrace))
           {
               AdvanceTokens();
               return result;
           }
           result.Add(ParseSingleMapPair());
           while (PeekTokenIs(TokenType.Comma))
           {
               AdvanceTokens();
               AdvanceTokens();
               result.Add(ParseSingleMapPair());
           }

           ExpectPeek(TokenType.RBrace);
           return result;
        }

        KeyValuePair<Expression, Expression> ParseSingleMapPair()
        {
            var key = ParseExpression(Precedence.Lowest);
            ExpectPeek(TokenType.Colon);
            AdvanceTokens();
            var value = ParseExpression(Precedence.Lowest);
            return new KeyValuePair<Expression, Expression>(key, value);
        }
    }

    private ArrayExpression ParseArrayLiteral()
    {
        var token = _currentToken;
        var elements = ParseExpressionList();
        return new ArrayExpression(token, elements);

        List<Expression> ParseExpressionList()
        {
            var list = new List<Expression>();

            if (PeekTokenIs(TokenType.RSquare))
            {
                AdvanceTokens();
                return list;
            }

            AdvanceTokens();
            list.Add(ParseExpression(Precedence.Lowest));

            while (PeekTokenIs(TokenType.Comma))
            {
                AdvanceTokens();
                AdvanceTokens();
                list.Add(ParseExpression(Precedence.Lowest));
            }

            ExpectPeek(TokenType.RSquare);
            
            return list;
        }
    }


    private PrefixExpression ParsePrefixExpression()
    {
        var @operator = _currentToken.Literal;
        AdvanceTokens();
        var operand = ParseExpression(Precedence.Prefix);
        return new PrefixExpression(_currentToken, operand, @operator);
    }

    private NullExpression ParseNullLiteral() => new(_currentToken);
    private StringExpression ParseStringLiteral() => new(_currentToken);
    private IdentifierExpression ParseIdentifier() => new(_currentToken);
    private NumberExpression ParseNumberLiteral() => new(_currentToken, long.Parse(_currentToken.Literal));
    private BooleanExpression ParseBooleanLiteral() => new(_currentToken);

    private Func<Expression, Expression> MapInfix(TokenType tokenType) => tokenType switch
    {
        TokenType.Plus or
            TokenType.Minus or
            TokenType.Asterisk or
            TokenType.Slash or
            TokenType.Gt or
            TokenType.Gte or
            TokenType.Eq or
            TokenType.NotEq or
            TokenType.Lt or
            TokenType.Lte => ParseInfixExpression,
        TokenType.LParen => ParseCallExpression,
        TokenType.LSquare => ParseIndexExpression,
        TokenType.Assign => ParseAssignExpression,
        _ => throw new UnExpectedTokenException(
            $"Parsing Error : Expected +,,*,/,>,>=,==,!=,<,<=, but got {_currentToken.Literal}!"),
    };

    private AssignmentExpression ParseAssignExpression(Expression left)
    {
        if (left is not IdentifierExpression)
            throw new UnExpectedTokenException( $"Parsing Error : Expected an identifier but got {left}!");
        
        var token = _currentToken;
        AdvanceTokens();
        var value = ParseExpression(Precedence.Lowest);
        return new AssignmentExpression(token, left, value);
    }

    private IndexExpression ParseIndexExpression(Expression array)
    {
        AdvanceTokens();
        var index = ParseExpression(Precedence.Lowest);
        ExpectPeek(TokenType.RSquare);
        return new IndexExpression(_currentToken, array, index);
    }

    private InfixExpression ParseInfixExpression(Expression left)
    {
        var current = _currentToken;
        var precedence = CurrentPrecedence();
        AdvanceTokens();
        var right = ParseExpression(precedence);
        return new InfixExpression(current, left, current.Literal, right);
    }

    private void AdvanceTokens()
    {
        _currentToken = _peekToken;
        _peekToken = _lexer.NextToken();
    }
    private int CurrentPrecedence() => Precedence.Map.GetValueOrDefault(_currentToken.Type, Precedence.Lowest);
    private int PeekPrecedence() => Precedence.Map.GetValueOrDefault(_peekToken.Type, Precedence.Lowest);
    private bool CurrentTokenIs(TokenType tokenType) => _currentToken.Type == tokenType;
    private bool PeekTokenIs(TokenType tokenType) => _peekToken.Type == tokenType;

    private bool ExpectPeek(TokenType tokenType)
    {
        if (!PeekTokenIs(tokenType)) throw new UnExpectedTokenException(tokenType, _peekToken.Type);
        AdvanceTokens();
        return true;
    }
}