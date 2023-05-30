namespace Gratinado.Compiler;
public partial class Parser
{
  private Expression? ParsePrimaryExpression()
  {
    Expression? expression = null
      ?? ParseParenthesisExpression() as Expression
      ?? ParseBlockExpression() as Expression
      ?? ParseUnaryOperatorExpression() as Expression
      ?? ParseLiteralExpression();

    return expression;
  }
}