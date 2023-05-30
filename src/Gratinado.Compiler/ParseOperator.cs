
namespace Gratinado.Compiler;
public partial class Parser
{
  private BinaryOperator? ParseBinaryOperator()
  {
    var nextToken = Peek();
    BinaryOperator? binaryOperator = nextToken switch
    {
      PlusToken => new AdditionOperator(nextToken),
      MinusToken => new SubtractionOperator(nextToken),
      AsteriskToken => new MultiplicationOperator(nextToken),
      ForwardSlashToken => new DivisionOperator(nextToken),
      QuestionForwardSlashToken => new SafeDivisionOperator(nextToken),
      DoubleQuestionMarkToken => new NullCoalescingOperator(nextToken),
      EqualsToken => new AssignmentOperator(nextToken),
      DoubleEqualsToken => new EqualityOperator(nextToken),
      ExclamationEqualsToken => new InequalityOperator(nextToken),
      GreaterThanToken => new GreaterThanOperator(nextToken),
      GreaterThanOrEqualToken => new GreaterThanOrEqualOperator(nextToken),
      LowerThanToken => new LowerThanOperator(nextToken),
      LowerThanOrEqualToken => new LowerThanOrEqualOperator(nextToken),
      _ => null
    };
    if (binaryOperator is not null)
    {
      ReadNextToken();
    }
    return binaryOperator;
  }

  private UnaryOperator? ParseUnaryOperator()
  {
    var nextToken = Peek();
    UnaryOperator? unaryOperator = nextToken switch
    {
      PlusToken => new IdentityOperator(nextToken),
      MinusToken => new NegationOperator(nextToken),
      ExclamationMarkToken => new LogicalNegationOperator(nextToken),
      _ => null
    };
    if (unaryOperator is not null)
    {
      ReadNextToken();
    }
    return unaryOperator;
  }
}