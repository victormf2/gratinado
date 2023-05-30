namespace Gratinado.Compiler;

public partial class Parser
{
  private UnaryOperatorExpression? ParseUnaryOperatorExpression()
  {
    var unaryOperator = ParseUnaryOperator();
    if (unaryOperator is null)
    {
      return null;
    }
    var unaryOperand = ParsePrimaryExpression();
    if (unaryOperand is null)
    {
      Diagnostics.Add(new ExpressionExpectedDiagnostic(_position + 1));
    }
    return new UnaryOperatorExpression(unaryOperator, unaryOperand);
  }
}