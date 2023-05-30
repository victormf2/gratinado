namespace Gratinado.Compiler
{
  public class RightOperandShouldNotBeNullException : Exception
  {

    public BinaryOperatorExpression Expression { get; }
    public RightOperandShouldNotBeNullException(
        BinaryOperatorExpression expression
    )
    {
      Expression = expression;
    }
  }
}