
namespace Gratinado.Compiler
{
  public interface IExpression : ISyntaxNode { }
  public static class ExpressionExtensions
  {
    public static bool IsValidExpression(this IExpression expression)
    {
      return expression is not InvalidExpression and not EOFExpression;
    }
  }
  public class EOFExpression : IExpression
  {
    public int Start => Token.Start;

    public int End => Token.End;

    public List<ISyntaxNode> Children { get; }

    public EOFToken Token { get; }

    public EOFExpression(EOFToken token)
    {
      Token = token;
      Children = new() { Token };
    }

    public bool EqualsIgnorePosition(ISyntaxNode node)
    {
      return node is EOFExpression;
    }
  }
  public class InvalidExpression : IExpression {
    public ISyntaxNode Node { get; }

    public int Start => Node.Start;

    public int End => Node.End;

    public List<ISyntaxNode> Children { get; }

    public InvalidExpression(ISyntaxNode node)
    {
      Node = node;
      Children = new() { Node };
    }

    public bool EqualsIgnorePosition(ISyntaxNode node)
    {
      return node is InvalidExpression other && other.Node.EqualsIgnorePosition(Node);
    }
  }
  public class LiteralExpression : IExpression
  {
    public ISyntaxNode Literal { get; }

    public List<ISyntaxNode> Children { get; }

    public int Start => Literal.Start;

    public int End => Literal.End;

    public LiteralExpression(ISyntaxNode literal)
    {
      Literal = literal;
      Children = new() { Literal };
    }

    public bool EqualsIgnorePosition(ISyntaxNode node)
    {
      return node is LiteralExpression other && other.Literal.EqualsIgnorePosition(Literal);
    }
  }

  public class BinaryOperatorExpression : IExpression
  {
    public List<ISyntaxNode> Children { get; }
    public IExpression LeftOperand { get; }
    public IExpression RightOperand { get; }
    public SyntaxToken Operator { get; }

    public int Start => LeftOperand.Start;

    public int End => RightOperand.End;

    public BinaryOperatorExpression(IExpression leftOperand, SyntaxToken op, IExpression rightOperand)
    {
      LeftOperand = leftOperand;
      RightOperand = rightOperand;
      Operator = op;
      Children = new() { LeftOperand, Operator, RightOperand };
    }

    public bool EqualsIgnorePosition(ISyntaxNode node)
    {
      return node is BinaryOperatorExpression other
        && other.LeftOperand.EqualsIgnorePosition(LeftOperand)
        && other.Operator.EqualsIgnorePosition(Operator)
        && other.RightOperand.EqualsIgnorePosition(RightOperand);
    }
  }

  public class UnaryOperatorExpression : IExpression
  {
    public List<ISyntaxNode> Children { get; }
    public ISyntaxNode Operand { get; }
    public ISyntaxNode Operator { get; }

    public int Start => Operator.Start;

    public int End => Operand.End;

    public UnaryOperatorExpression(ISyntaxNode Operand, ISyntaxNode op)
    {
      this.Operand = Operand;
      Operator = op;
      Children = new() { this.Operand, Operator };
    }

    public bool EqualsIgnorePosition(ISyntaxNode node)
    {
      return node is UnaryOperatorExpression other
        && other.Operand.EqualsIgnorePosition(Operand)
        && other.Operator.EqualsIgnorePosition(Operator);
    }
  }



  public class ParenthesisExpression : IExpression
  {
    public ISyntaxNode OpenParenthesis { get; }
    public IExpression Expression { get; }
    public ISyntaxNode CloseParenthesis { get; }
    public ParenthesisExpression(ISyntaxNode openParenthesis, IExpression expression, ISyntaxNode closeParenthesis)
    {
      OpenParenthesis = openParenthesis;
      Expression = expression;
      CloseParenthesis = closeParenthesis;
      Children = new() { OpenParenthesis, Expression, closeParenthesis };
    }
    public List<ISyntaxNode> Children { get; }

    public int Start => OpenParenthesis.Start;

    public int End => CloseParenthesis.End;

    public override string ToString()
    {
      return $"{OpenParenthesis}{Expression}{CloseParenthesis}";
    }

    public bool EqualsIgnorePosition(ISyntaxNode node)
    {
      return node is ParenthesisExpression other
        && other.OpenParenthesis.EqualsIgnorePosition(OpenParenthesis)
        && other.Expression.EqualsIgnorePosition(Expression)
        && other.CloseParenthesis.EqualsIgnorePosition(CloseParenthesis);
    }
  }

  public class BlockExpression : IExpression
  {
    public OpenCurlyBracketsToken OpenBlock { get; }
    public List<IExpression> Expressions { get; }
    public CloseCurlyBracketsToken CloseBlock { get; }

    public BlockExpression(OpenCurlyBracketsToken openBlock, List<IExpression> expressions, CloseCurlyBracketsToken closeBlock)
    {
      OpenBlock = openBlock;
      Expressions = expressions;
      CloseBlock = closeBlock;
      Children = new List<ISyntaxNode> { OpenBlock }.Concat(Expressions).Append(CloseBlock).ToList();
    }

    public List<ISyntaxNode> Children { get; }

    public int Start => OpenBlock.Start;

    public int End => CloseBlock.End;

    public bool EqualsIgnorePosition(ISyntaxNode node)
    {
      return node is BlockExpression other
        && other.OpenBlock.EqualsIgnorePosition(OpenBlock)
        && other.Expressions.Count == Expressions.Count
        && other.Expressions
          .Select((otherExpression, index) => (otherExpression, index))
          .All(i => i.otherExpression.EqualsIgnorePosition(Expressions[i.index]))
        && other.CloseBlock.EqualsIgnorePosition(CloseBlock);
    }
  }
}