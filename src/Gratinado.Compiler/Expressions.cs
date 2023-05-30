
namespace Gratinado.Compiler
{
  public abstract class Expression : ISyntaxNode
  {
    public int Start { get; private set; } = -1;

    public int End { get; private set; } = -1;

    private readonly List<ISyntaxNode> _children = new();
    public IEnumerable<ISyntaxNode> Children => _children;

    public bool EqualsIgnorePosition(ISyntaxNode? other)
    {
      if (other is null || other.GetType() != GetType())
      {
        return false;
      }
      var otherChildren = other.Children.ToList();
      var thisChildren = Children.ToList();

      return other is not null
        && other.GetType() == GetType()
        && otherChildren.Count == thisChildren.Count
        && otherChildren
          .Select((child, index) => (child, index))
          .All(i => i.child.EqualsIgnorePosition(thisChildren[i.index]));
    }

    protected void AddChildren(params ISyntaxNode?[] children)
    {
      _children.AddRange(
        children.Where(child => child is not null)
      );

      if (_children.Count > 0)
      {
        Start = _children[0].Start;
        End = _children[^1].End;
      }
    }
  }

  public class LiteralExpression : Expression
  {
    public ISyntaxNode Literal { get; }

    public LiteralExpression(ISyntaxNode literal)
    {
      Literal = literal;
      AddChildren(Literal);
    }

    public override string ToString()
    {
      return $"{Literal}";
    }
  }

  public class BinaryOperatorExpression : Expression
  {
    public Expression LeftOperand { get; }
    public BinaryOperator Operator { get; }
    public Expression? RightOperand { get; }

    public BinaryOperatorExpression(Expression leftOperand, BinaryOperator op, Expression? rightOperand)
    {
      LeftOperand = leftOperand;
      Operator = op;
      RightOperand = rightOperand;
      AddChildren(LeftOperand, Operator, RightOperand);
    }

    public override string ToString()
    {
      return $"{LeftOperand} {Operator} {RightOperand}";
    }
  }

  public class UnaryOperatorExpression : Expression
  {
    public ISyntaxNode Operator { get; }
    public Expression? Operand { get; }

    public UnaryOperatorExpression(ISyntaxNode op, Expression? operand)
    {
      Operator = op;
      Operand = operand;
      AddChildren(Operator, Operand);
    }

    public override string ToString()
    {
      return $"{Operator}{Operand}";
    }
  }

  public class ParenthesisExpression : Expression
  {
    public OpenParenthesisToken OpenParenthesis { get; }
    public Expression? Expression { get; }
    public CloseParenthesisToken? CloseParenthesis { get; }
    public ParenthesisExpression(OpenParenthesisToken openParenthesis, Expression? expression, CloseParenthesisToken? closeParenthesis)
    {
      OpenParenthesis = openParenthesis;
      Expression = expression;
      CloseParenthesis = closeParenthesis;
      AddChildren(OpenParenthesis, Expression, CloseParenthesis);
    }
    public override string ToString()
    {
      return $"{OpenParenthesis}{Expression}{CloseParenthesis}";
    }
  }

  public class BlockExpression : Expression
  {
    public OpenCurlyBracketsToken OpenBlock { get; }
    public List<Expression> Expressions { get; }
    public CloseCurlyBracketsToken? CloseBlock { get; }

    public BlockExpression(OpenCurlyBracketsToken openBlock, List<Expression> expressions, CloseCurlyBracketsToken? closeBlock)
    {
      OpenBlock = openBlock;
      Expressions = expressions;
      CloseBlock = closeBlock;
      AddChildren(OpenBlock);
      AddChildren(Expressions.ToArray());
      AddChildren(CloseBlock);
    }

    public override string ToString()
    {
      return $"{OpenBlock}{string.Join(";", Expressions)}{CloseBlock}";
    }
  }
}