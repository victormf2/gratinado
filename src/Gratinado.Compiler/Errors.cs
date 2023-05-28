using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gratinado.Compiler
{
  public interface IError { }
  public class ExpectedExpressionError : IError
  {
    public ISyntaxNode Expression { get; }
    public ExpectedExpressionError(ISyntaxNode expression)
    {
      Expression = expression;
    }
  }

  public class InvalidTokenError : IError
  {
    public SyntaxToken Token { get; }
    public InvalidTokenError(SyntaxToken token)
    {
      Token = token;
    }
  }

  public class ExpectedOperatorError : IError
  {
    public IValueExpression Left { get; }
    public IValueExpression Right { get; }
    public ExpectedOperatorError(IValueExpression left, IValueExpression right)
    {
      Left = left;
      Right = right;
    }
  }

  public class ExpectedRightOperandError : IError
  {
    public IValueExpression Left { get; }
    public IOperator Operator { get; }
    public ExpectedRightOperandError(IValueExpression left, IOperator @operator)
    {
      Left = left;
      Operator = @operator;
    }
  }

  public class ExpectedLeftOperandError : IError
  {
    public IValueExpression Right { get; }
    public IOperator Operator { get; }
    public ExpectedLeftOperandError(IValueExpression right, IOperator @operator)
    {
      Right = right;
      Operator = @operator;
    }
  }

  public class ExcpectedOperandsError : IError
  {
    public IOperator Operator { get; }
    public ExcpectedOperandsError(IOperator @operator)
    {
      Operator = @operator;
    }
  }

  public class ExpectOperatorAndOperandsError : IError
  {
    
  }

  public class ExpectedCloseParenthesisError : IError
  {
      public OpenParenthesis OpenParenthesis { get; }
      public IValueExpression Expression { get; }
      public ISyntaxNode NextParsedToken { get; }

      public ExpectedCloseParenthesisError(OpenParenthesis openParenthesis, IValueExpression expression, ISyntaxNode nextParsedToken)
      {
          OpenParenthesis = openParenthesis;
          Expression = expression;
          NextParsedToken = nextParsedToken;
      }
  }

  public class ExpectedCloseBlockError : IError
  {
      public OpenBlock OpenBlock { get; }
      public List<IValueExpression> Expressions { get; }
      public ISyntaxNode NextParsedToken { get; }

      public ExpectedCloseBlockError(OpenBlock openBlock, List<IValueExpression> expressions, ISyntaxNode nextParsedToken)
      {
          OpenBlock = openBlock;
          Expressions = expressions;
          NextParsedToken = nextParsedToken;
      }
  }
}