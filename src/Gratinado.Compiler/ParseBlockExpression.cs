namespace Gratinado.Compiler;
public partial class Parser
{
  private Expression ParseBlockExpression(OpenCurlyBracketsToken openCurlyBracketsToken)
  {
    var expressions = new List<Expression>();
    var currentExpression = ParseExpression();

    while (currentExpression is not null)
    {
      expressions.Add(currentExpression);
      currentExpression = ParseExpression();
    }

    var nextToken = Peek();
    if (nextToken is CloseCurlyBracketsToken closeCurlyBracketsToken)
    {
      ReadNextToken();
      return new BlockExpression(openCurlyBracketsToken, expressions, closeCurlyBracketsToken);
    }

    Diagnostics.Add(
      new CloseCurlyBracketsExpectedDiagnostic(_position + 1)
    );

    return new BlockExpression(openCurlyBracketsToken, expressions, null);
  }
}