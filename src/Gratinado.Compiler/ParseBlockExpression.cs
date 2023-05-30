namespace Gratinado.Compiler;
public partial class Parser
{
  private BlockExpression? ParseBlockExpression()
  {
    var openCurlyBracketsToken = Match<OpenCurlyBracketsToken>();
    if (openCurlyBracketsToken is null)
    {
      return null;
    }
    var expressions = new List<Expression>();
    var currentExpression = ParseExpression();
    while (currentExpression is not null)
    {
      expressions.Add(currentExpression);
      currentExpression = ParseExpression();
    }

    var closeCurlyBracketsToken = Match<CloseCurlyBracketsToken>();
    if (closeCurlyBracketsToken is null)
    {
      Diagnostics.Add(
        new CloseCurlyBracketsExpectedDiagnostic(_position + 1)
      );
    }
    return new BlockExpression(openCurlyBracketsToken, expressions, closeCurlyBracketsToken);
  }
}