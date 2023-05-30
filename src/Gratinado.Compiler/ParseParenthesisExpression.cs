namespace Gratinado.Compiler;
public partial class Parser
{
  private Expression ParseParenthesisExpression(
    OpenParenthesisToken openParenthesis
  )
  {
    var expression = ParseExpression();

    var nextToken = Peek();
    if (nextToken is CloseParenthesisToken closeParenthesisToken)
    {
      ReadNextToken();
      return new ParenthesisExpression(openParenthesis, expression, closeParenthesisToken);
    }

    Diagnostics.Add(
      new CloseParenthesisExpectedDiagnostic(_position + 1)
    );

    return new ParenthesisExpression(openParenthesis, expression, null);
  }
}