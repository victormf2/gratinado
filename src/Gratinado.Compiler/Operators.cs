namespace Gratinado.Compiler;

public abstract class Operator : ISyntaxNode
{
  public SyntaxToken Token { get; }
  public int Start => Token.Start;

  public int End => Token.End;

  public IEnumerable<ISyntaxNode> Children { get; }
  
  public Operator(SyntaxToken token)
  {
    Token = token;
    Children = new List<ISyntaxNode> { Token };
  }

  public bool EqualsIgnorePosition(ISyntaxNode? other)
  {
    return other is Operator op 
      && op.GetType() == GetType() 
      && op.Token.EqualsIgnorePosition(Token);
  }

  public override string ToString()
  {
    return $"{Token}";
  }
}

public abstract class BinaryOperator : Operator
{
  public abstract Precedences Precedence { get; }
  public abstract Associativities Associativity { get; }
  protected BinaryOperator(SyntaxToken token) : base(token)
  {
  }
}

public class AdditionOperator : BinaryOperator
{
  public override Precedences Precedence => Precedences.Additive;
  public override Associativities Associativity => Associativities.LeftToRight;
  public AdditionOperator(SyntaxToken token) : base(token)
  {
  }

}
public class SubtractionOperator : BinaryOperator
{
  public override Precedences Precedence => Precedences.Additive;
  public override Associativities Associativity => Associativities.LeftToRight;
  public SubtractionOperator(SyntaxToken token) : base(token)
  {
  }
}
public class MultiplicationOperator : BinaryOperator
{
  public override Precedences Precedence => Precedences.Multiplicative;
  public override Associativities Associativity => Associativities.LeftToRight;
  public MultiplicationOperator(SyntaxToken token) : base(token)
  {
  }
}
public class DivisionOperator : BinaryOperator
{
  public override Precedences Precedence => Precedences.Multiplicative;
  public override Associativities Associativity => Associativities.LeftToRight;
  public DivisionOperator(SyntaxToken token) : base(token)
  {
  }
}
public class SafeDivisionOperator : BinaryOperator
{
  public override Precedences Precedence => Precedences.Multiplicative;
  public override Associativities Associativity => Associativities.LeftToRight;
  public SafeDivisionOperator(SyntaxToken token) : base(token)
  {
  }
}
public class NullCoalescingOperator : BinaryOperator
{
  public override Precedences Precedence => Precedences.NullCoalescingOperator;
  public override Associativities Associativity => Associativities.RightToLeft;
  public NullCoalescingOperator(SyntaxToken token) : base(token)
  {
  }
}
public class AssignmentOperator : BinaryOperator
{
  public override Associativities Associativity => Associativities.RightToLeft;
  public override Precedences Precedence => Precedences.AssignmentAndLambdaDeclarations;
  public AssignmentOperator(SyntaxToken token) : base(token)
  {
  }
}
public class EqualityOperator : BinaryOperator
{
  public override Associativities Associativity => Associativities.LeftToRight;
  public override Precedences Precedence => Precedences.EqualityComparison;
  public EqualityOperator(SyntaxToken token) : base(token)
  {
  }
}
public class InequalityOperator : BinaryOperator
{
  public override Associativities Associativity => Associativities.LeftToRight;
  public override Precedences Precedence => Precedences.EqualityComparison;
  public InequalityOperator(SyntaxToken token) : base(token)
  {
  }
}
public class GreaterThanOperator : BinaryOperator
{
  public override Associativities Associativity => Associativities.LeftToRight;
  public override Precedences Precedence => Precedences.RelationalAndTypeTesting;
  public GreaterThanOperator(SyntaxToken token) : base(token)
  {
  }
}
public class GreaterThanOrEqualOperator : BinaryOperator
{
  public override Associativities Associativity => Associativities.LeftToRight;
  public override Precedences Precedence => Precedences.RelationalAndTypeTesting;
  public GreaterThanOrEqualOperator(SyntaxToken token) : base(token)
  {
  }
}
public class LowerThanOperator : BinaryOperator
{
  public override Associativities Associativity => Associativities.LeftToRight;
  public override Precedences Precedence => Precedences.RelationalAndTypeTesting;
  public LowerThanOperator(SyntaxToken token) : base(token)
  {
  }
}
public class LowerThanOrEqualOperator : BinaryOperator
{
  public override Associativities Associativity => Associativities.LeftToRight;
  public override Precedences Precedence => Precedences.RelationalAndTypeTesting;
  public LowerThanOrEqualOperator(SyntaxToken token) : base(token)
  {
  }
}

public abstract class UnaryOperator : Operator
{
  protected UnaryOperator(SyntaxToken token) : base(token)
  {
  }
}

public class IdentityOperator : UnaryOperator
{
  public IdentityOperator(SyntaxToken token) : base(token)
  {
  }
}
public class NegationOperator : UnaryOperator
{
  public NegationOperator(SyntaxToken token) : base(token)
  {
  }
}
public class LogicalNegationOperator : UnaryOperator
{
  public LogicalNegationOperator(SyntaxToken token) : base(token)
  {
  }
}