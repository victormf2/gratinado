namespace Gratinado.Compiler;
public partial class Parser
{
  private LiteralExpression? ParseLiteralExpression()
  {
    var nextToken = Peek();
    if (nextToken is NumberToken or StringToken)
    {
      ReadNextToken();
      return new LiteralExpression(nextToken);
    }
    return null;
  }
}