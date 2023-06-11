namespace Gratinado.Compiler;
public partial class Parser
{
  private Expression? ParsePrimaryExpression()
  {
    Expression? expression = null
      ?? ParseParenthesisExpression() as Expression
      ?? ParseBlockExpression() as Expression
      ?? ParseFunctionDeclaration() as Expression
      ?? ParseUnaryOperatorExpression() as Expression
      ?? ParseLiteralExpression() as Expression;

    return expression;
  }
}