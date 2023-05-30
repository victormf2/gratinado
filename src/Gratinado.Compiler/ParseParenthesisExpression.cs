namespace Gratinado.Compiler;
public partial class Parser
{
  private ParenthesisExpression? ParseParenthesisExpression()
  {

    var openParenthesisToken = Match<OpenParenthesisToken>();

    if (openParenthesisToken is null)
    {
      return null;
    }

    CloseParenthesisToken? closeParenthesisToken;
    
    var expression = ParseExpression();
    if (expression is null)
    {
      Diagnostics.Add(new ExpressionExpectedDiagnostic(_position + 1));
      closeParenthesisToken = Match<CloseParenthesisToken>();
      return new ParenthesisExpression(
        openParenthesisToken,
        expression,
        closeParenthesisToken
      );
    }

    closeParenthesisToken = Match<CloseParenthesisToken>();
    if (closeParenthesisToken is null)
    {
      Diagnostics.Add(
        new CloseParenthesisExpectedDiagnostic(_position + 1)
      );
    }

    return new ParenthesisExpression(
      openParenthesisToken,
      expression,
      closeParenthesisToken
    );
  }
}