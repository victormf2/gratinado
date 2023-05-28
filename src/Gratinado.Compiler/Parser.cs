namespace Gratinado.Compiler
{

  public class InvalidExpression : ISyntaxNode, IValueExpression
  {

    public ISyntaxNode Expression { get; }

    public InvalidExpression(ISyntaxNode expression)
    {
      Expression = expression;
      Children = new List<ISyntaxNode> { Expression };
    }
    public List<ISyntaxNode> Children { get; }

    public Prescedences Prescedence => Prescedences.Literals;

    public override string ToString()
    {
      return Expression.ToString() ?? "";
    }

  }

  public class InvalidCloseParenthesis : ISyntaxNode, ICloseParenthesis
  {

    public ISyntaxNode Expression { get; }

    public InvalidCloseParenthesis(ISyntaxNode expression)
    {
      Expression = expression;
      Children = new List<ISyntaxNode> { Expression };
    }
    public List<ISyntaxNode> Children { get; }

    public override string ToString()
    {
      return Expression.ToString() ?? "";
    }

  }

  public class InvalidOperator : ISyntaxNode, IOperator
  {
    public ISyntaxNode Expression { get; }
    public InvalidOperator(ISyntaxNode expression)
    {
      Expression = expression;
      Children = new List<ISyntaxNode>() { Expression };
    }
    public List<ISyntaxNode> Children { get; }

    public Prescedences Prescedence => Prescedences.Literals;

    public override string ToString()
    {
      return Expression.ToString() ?? "";
    }

  }

  public class BinaryOperator : IValueExpression
  {
    public IValueExpression Left { get; }
    public IOperator Operator { get; }
    public IValueExpression Right { get; }
    public BinaryOperator(IValueExpression left, IOperator @operator, IValueExpression right)
    {
      Left = left;
      Operator = @operator;
      Right = right;
      Children = new List<ISyntaxNode> { Left, Operator, Right };
    }
    public List<ISyntaxNode> Children { get; }

    public Prescedences Prescedence => Operator.Prescedence;

    public override string ToString()
    {
      return $"({Left} {Operator} {Right})";
    }
  }

  public class EmptyValueExpression : IValueExpression
  {
    public List<ISyntaxNode> Children { get; } = new();

    public Prescedences Prescedence => Prescedences.Literals;
  }

  public class EmptyOperator : IOperator
  {
    public List<ISyntaxNode> Children { get; } = new();

    public Prescedences Prescedence => Prescedences.Literals;
  }

  public class ParenthesisExpression : IValueExpression
  {
    public OpenParenthesis OpenParenthesis { get; }
    public IValueExpression Expression { get; }
    public ICloseParenthesis CloseParenthesis { get; }
    public ParenthesisExpression(OpenParenthesis openParenthesis, IValueExpression expression, ICloseParenthesis closeParenthesis)
    {
      OpenParenthesis = openParenthesis;
      Expression = expression;
      CloseParenthesis = closeParenthesis;
      Children = new() { OpenParenthesis, Expression, closeParenthesis };
    }
    public List<ISyntaxNode> Children { get; }

    public Prescedences Prescedence => Prescedences.Parenthesis;

    public override string ToString()
    {
      return $"{OpenParenthesis}{Expression}{CloseParenthesis}";
    }
  }

  public class BlockExpression : IValueExpression
  {
    private readonly string _indent;
    public OpenBlock OpenBlock { get; }
    public List<IValueExpression> Expressions { get; }
    public ICloseBlock CloseBlock { get; }

    public BlockExpression(int indentLevel, OpenBlock openBlock, List<IValueExpression> expressions, ICloseBlock closeBlock)
    {
      _indent = string.Concat(Enumerable.Repeat("  ", indentLevel)); ;
      OpenBlock = openBlock;
      Expressions = expressions;
      CloseBlock = closeBlock;
      Children = new List<ISyntaxNode> { OpenBlock }.Concat(Expressions).Append(CloseBlock).ToList();
    }

    public List<ISyntaxNode> Children { get; }

    public Prescedences Prescedence => Prescedences.Block;

    public override string ToString()
    {
      return $"{OpenBlock}\n  {_indent}{string.Join("\n  " + _indent, Expressions)}\n{_indent}{CloseBlock}";
    }
  }

  public class InvalidCloseBlock : ICloseBlock
  {
    public ISyntaxNode Expression { get; }

    public InvalidCloseBlock(ISyntaxNode expression)
    {
      Expression = expression;
      Children = new List<ISyntaxNode> { Expression };
    }

    public List<ISyntaxNode> Children { get; }

    public override string ToString()
    {
      return Expression.ToString() ?? "";
    }
  }


  public class Parser
  {
    public List<IError> Errors { get; } = new();
    public List<IValueExpression> Expressions = new();
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
      while (currentExpression is not EOF)
      {

        if (currentExpression is IValueExpression valueExpression)
        {
          Expressions.Add(valueExpression);
        }
        else
        {
          Errors.Add(
            new ExpectedExpressionError(currentExpression)
          );

          Expressions.Add(new InvalidExpression(currentExpression));
        }

        currentExpression = ParseExpression();
      }
    }



    private ISyntaxNode ParseExpression(int indentLevel = 0)
    {
      var currentParsedToken = ReadNextToken();
      if (currentParsedToken is EOF)
      {
        return currentParsedToken;
      }

      IValueExpression? accumulatedExpression = null;
      IOperator? @operator = null;

      while (true)
      {

        if (currentParsedToken is OpenParenthesis openParenthesis)
        {
          accumulatedExpression = ApplyPrescedence(
            left: accumulatedExpression,
            @operator: @operator,
            right: ParseParenthesisExpression(openParenthesis)
          );
        }
        else if (currentParsedToken is CloseParenthesis)
        {
          return currentParsedToken;
        }
        else if (currentParsedToken is OpenBlock openBlock)
        {
          accumulatedExpression = ApplyPrescedence(
            left: accumulatedExpression, 
            @operator: @operator, 
            right: ParseBlockExpression(indentLevel, openBlock)
          );
        }
        else if (currentParsedToken is CloseBlock)
        {
          return currentParsedToken;
        }
        else if (currentParsedToken is IValueExpression valueExpression)
        {
          accumulatedExpression = ApplyPrescedence(
            left: accumulatedExpression,
            @operator: @operator,
            right: valueExpression
          );
        }
        else
        {
          Errors.Add(
            new InvalidTokenError(
              token: currentParsedToken
            )
          );
          return new InvalidExpression(currentParsedToken);
        }

        var nextParsedToken = ReadNextToken();

        if (nextParsedToken is not IOperator nextParsedOperator)
        {
          _position -= 1;
          return accumulatedExpression;
        }
        @operator = nextParsedOperator;
        currentParsedToken = ReadNextToken();
      }
    }
    private SyntaxToken ReadNextToken()
    {
      var nextToken = _tokens[_position];
      _position += 1;
      return nextToken;
    }

    private IValueExpression ApplyPrescedence(IValueExpression? left, IOperator? @operator, IValueExpression right)
    {
      if (@operator is not null && left is BinaryOperator binaryOperator)
      {
        if (@operator.Prescedence > left.Prescedence)
        {
          var expressionWithInvertedPrescedence = new BinaryOperator(
            binaryOperator.Left,
            binaryOperator.Operator,
            new BinaryOperator(
              binaryOperator.Right,
              @operator,
              right
            )
          );
          return expressionWithInvertedPrescedence;
        }
      }

      var expressionWithNaturalPrescedence = ParseOperatorExpression(
        left: left,
        @operator,
        right: right
      );

      return expressionWithNaturalPrescedence;
    }

    private IValueExpression ParseOperatorExpression(
      IValueExpression? left,
      IOperator? @operator,
      IValueExpression? right
    )
    {

      if (left is null)
      {
        if (@operator is null)
        {
          if (right is null)
          {
            Errors.Add(new ExpectOperatorAndOperandsError());
          }
          else
          {
            return right;
          }
        }
        else /* operator is not null */
        {
          if (right is null)
          {
            Errors.Add(new ExcpectedOperandsError(@operator));
          }
          else
          {
            Errors.Add(new ExpectedLeftOperandError(right, @operator));
          }
        }
      }
      else /* left is not null */
      {
        if (@operator is null)
        {
          if (right is null)
          {
            return left;
          }
          else
          {
            Errors.Add(new ExpectedOperatorError(left, right));
          }
        }
        else /* operator is not null */
        {
          if (right is null)
          {
            Errors.Add(new ExpectedRightOperandError(left, @operator));
          }
        }
      }

      return new BinaryOperator(
        left: left ?? new EmptyValueExpression(),
        @operator: @operator ?? new EmptyOperator(),
        right: right ?? new EmptyValueExpression()
      );

    }

    private ParenthesisExpression ParseParenthesisExpression(
      OpenParenthesis openParenthesis
    )
    {
      var expression = ParseExpression();

      if (expression is CloseParenthesis closeParenthesis)
      {
        Errors.Add(new ExpectedExpressionError(expression));

        return new ParenthesisExpression(
          openParenthesis,
          expression: new EmptyValueExpression(),
          closeParenthesis
        );
      }

      if (expression is not IValueExpression valueExpression)
      {
        Errors.Add(
          new ExpectedExpressionError(expression)
        );
        return ApplyCloseParenthesis(openParenthesis, new InvalidExpression(expression));
      }

      return ApplyCloseParenthesis(openParenthesis, valueExpression);
    }

    private ParenthesisExpression ApplyCloseParenthesis(OpenParenthesis openParenthesis, IValueExpression expression)
    {
      var nextParsedToken = ReadNextToken();
      if (nextParsedToken is CloseParenthesis closeParenthesis)
      {
        return new ParenthesisExpression(
          openParenthesis,
          expression,
          closeParenthesis
        );
      }

      Errors.Add(
        new ExpectedCloseParenthesisError(
          openParenthesis,
          expression,
          nextParsedToken
        )
      );
      _position -= 1;
      return new ParenthesisExpression(
        openParenthesis,
        expression,
        closeParenthesis: new InvalidCloseParenthesis(expression: nextParsedToken)
      );

    }
    private BlockExpression ParseBlockExpression(int indentLevel, OpenBlock openBlock)
    {
      var expressions = new List<IValueExpression>();
      var currentExpression = ParseExpression(indentLevel + 1);

      while (currentExpression is IValueExpression valueExpression)
      {
        expressions.Add(valueExpression);
        currentExpression = ParseExpression(indentLevel + 1);
      }

      if (currentExpression is CloseBlock closeBlock)
      {
        return new BlockExpression(indentLevel, openBlock, expressions, closeBlock);
      }

      Errors.Add(
        new ExpectedCloseBlockError(
          openBlock,
          expressions,
          currentExpression
        )
      );

      _position -= 1;

      return new BlockExpression(
        indentLevel,
        openBlock,
        expressions,
        closeBlock: new InvalidCloseBlock(currentExpression)
      );
    }
  }
}