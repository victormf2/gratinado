namespace Gratinado.Compiler
{
  public partial class Parser
  {
    public List<Diagnostic> Diagnostics { get; } = new();
    public List<IExpression> Expressions = new();
    private readonly List<SyntaxToken> _tokens = new();
    private int _position = 0;
    public Lexer Lexer { get; set; }
    public Parser(Lexer lexer)
    {
      Lexer = lexer;
      _tokens = lexer.ToList();
    }

    public void Parse()
    {
      var currentExpression = ParseExpression();
      while (currentExpression is not EOFExpression)
      {
        Expressions.Add(currentExpression);
        currentExpression = ParseExpression();
      }
    }

    private IExpression ParseExpressionFromRightToLeft()
    {
      var currentToken = ReadNextToken();
      if (currentToken is EOFToken eOFToken)
      {
        return new EOFExpression(eOFToken);
      }

      var left = ParsePrimaryExpression(currentToken);
      if (left.IsValidExpression())
      {
        
        var nextToken = ReadNextToken();
        if (nextToken.IsBinaryOperatorToken())
        {
          var associativity = nextToken.GetAssociativity();
          if (associativity == Associativity.FromRightToLeft)
          {
            var right = ParseExpressionFromRightToLeft();
            return new BinaryOperatorExpression(left, nextToken, right);
          }
        }
      }

      return left;
    }

    private IExpression ParseExpression(
      Precedence precedence = Precedence.Literals
    )
    {
      var currentToken = ReadNextToken();
      if (currentToken is EOFToken eOFToken)
      {
        return new EOFExpression(eOFToken);
      }

      var left = ParsePrimaryExpression(currentToken);
      while (left.IsValidExpression())
      {
        var nextToken = ReadNextToken();
        var operatorPrecedence = nextToken.GetPrecedence();
        var operatorAssociativity = nextToken.GetAssociativity();
        if (
          !nextToken.IsBinaryOperatorToken()
            || (
              operatorAssociativity == Associativity.FromRightToLeft && operatorPrecedence < precedence
            ) 
            || (
              operatorAssociativity == Associativity.FromLeftToRight && operatorPrecedence <= precedence
            )
        )
        {
          _position -= 1;
          return left;
        }

        var binaryOperatorToken = nextToken;

        var right = ParseExpression(operatorPrecedence);

        if (operatorAssociativity == Associativity.FromRightToLeft)
        {
          return new BinaryOperatorExpression(left, binaryOperatorToken, right);
        }

        left = new BinaryOperatorExpression(left, binaryOperatorToken, right);
      }

      return left;
    }
    private SyntaxToken ReadNextToken()
    {
      var nextToken = _tokens[_position];
      _position += 1;
      return nextToken;
    }
    private IExpression ParseParenthesisExpression(
      OpenParenthesisToken openParenthesis
    )
    {
      var expression = ParseExpression();

      if (expression is InvalidExpression invalidExpression)
      {

        if (expression is CloseParenthesisToken closeParenthesisToken)
        {
          return new ParenthesisExpression(
            openParenthesis,
            expression,
            closeParenthesisToken
          );
        }
      }

      return ApplyCloseParenthesis(openParenthesis, expression);
    }

    private IExpression ApplyCloseParenthesis(OpenParenthesisToken openParenthesis, IExpression expression)
    {
      var nextParsedToken = ReadNextToken();
      if (nextParsedToken is CloseParenthesisToken closeParenthesisToken)
      {
        return new ParenthesisExpression(
          openParenthesis,
          expression,
          closeParenthesisToken
        );
      }

      Diagnostics.Add(
        new CloseParenthesisExpectedDiagnostic(nextParsedToken)
      );

      return new InvalidExpression(openParenthesis);
    }
    private IExpression ParseBlockExpression(OpenCurlyBracketsToken openCurlyBracketsToken)
    {
      var expressions = new List<IExpression>();
      var currentExpression = ParseExpression();

      while (currentExpression is not CloseCurlyBracketsToken or EOFToken)
      {
        expressions.Add(currentExpression);
        currentExpression = ParseExpression();
      }

      if (currentExpression is CloseCurlyBracketsToken closeCurlyBracketsToken)
      {
        return new BlockExpression(openCurlyBracketsToken, expressions, closeCurlyBracketsToken);
      }

      Diagnostics.Add(
        new CloseCurlyBracketsExpectedDiagnostic(currentExpression)
      );

      return new InvalidExpression(openCurlyBracketsToken);
    }

    private IExpression ParsePrimaryExpression(SyntaxToken currentToken)
    {
      if (currentToken is OpenParenthesisToken openParenthesis)
      {
        return ParseParenthesisExpression(openParenthesis);
      }
      if (currentToken is OpenCurlyBracketsToken openBlock)
      {
        return ParseBlockExpression(openBlock);
      }
      if (currentToken.IsLiteralToken())
      {
        return new LiteralExpression(currentToken);
      }
      if (currentToken.IsUnaryOperatorToken())
      {
        return new UnaryOperatorExpression(@currentToken, ParsePrimaryExpression(ReadNextToken()));
      }

      Diagnostics.Add(new ExpressionExpectedDiagnostic(currentToken));
      return new InvalidExpression(currentToken);
    }
  }
}