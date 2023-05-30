namespace Gratinado.Compiler;
public partial class Parser
{
  private Expression? ParseExpression()
  {
    var leftOperandExpression = ParsePrimaryExpression();
    if (leftOperandExpression is null)
    {
      return null;
    }
    var binaryOperator = ParseBinaryOperator();
    if (binaryOperator is null)
    {
      return leftOperandExpression;
    }
    var rightOperandExpression = ParsePrimaryExpression();

    if (rightOperandExpression is null)
    {
      Diagnostics.Add(new ExpressionExpectedDiagnostic(_position + 1));
      return new BinaryOperatorExpression(leftOperandExpression, binaryOperator, null);
    }

    var binaryExpression = new BinaryOperatorExpression(leftOperandExpression, binaryOperator, rightOperandExpression);

    return ParseAccumulateBinaryExpression(binaryExpression);
  }

  private BinaryOperatorExpression ParseAccumulateBinaryExpression(BinaryOperatorExpression fullExpression)
  {
    var nextOperator = ParseBinaryOperator();
    while (nextOperator is not null)
    {
      var nextOperand = ParsePrimaryExpression();

      if (nextOperand is null)
      {
        Diagnostics.Add(new ExpressionExpectedDiagnostic(_position + 1));
      }

      var leftOperatorPrecedence = fullExpression.Operator.Precedence;
      var rightOperatorPrecedence = nextOperator.Precedence;
      if (
        leftOperatorPrecedence < rightOperatorPrecedence
      )
      {
        fullExpression = new BinaryOperatorExpression(
          fullExpression.LeftOperand,
          fullExpression.Operator,
          ParseAccumulateBinaryExpression(
            new BinaryOperatorExpression(
              fullExpression.RightOperand,
              nextOperator,
              nextOperand
            )
          )
        );
      }
      else if (leftOperatorPrecedence > rightOperatorPrecedence)
      {
        fullExpression = new BinaryOperatorExpression(
          fullExpression,
          nextOperator,
          nextOperand
        );
      }
      else if (nextOperator.Associativity is Associativities.RightToLeft)
      {
        fullExpression = new BinaryOperatorExpression(
          fullExpression.LeftOperand,
          fullExpression.Operator,
          ParseAccumulateBinaryExpression(
            new BinaryOperatorExpression(
              fullExpression.RightOperand,
              nextOperator,
              nextOperand
            )
          )
        );
      }
      else
      {
        fullExpression = new BinaryOperatorExpression(
          fullExpression,
          nextOperator,
          nextOperand
        );
      }

      nextOperator = ParseBinaryOperator();
    }

    return fullExpression;
  }


}