namespace Gratinado.Compiler;
public partial class Parser
{
  private Expression? ParsePrimaryExpression()
  {
    var startPosition = _position;
    var nextToken = ReadNextToken();
    if (nextToken is OpenParenthesisToken openParenthesis)
    {
      return ParseParenthesisExpression(openParenthesis);
    }
    if (nextToken is OpenCurlyBracketsToken openBlock)
    {
      return ParseBlockExpression(openBlock);
    }
    if (nextToken.IsLiteralToken())
    {
      return new LiteralExpression(nextToken);
    }
    _position = startPosition;
    var unaryOperatorExpression = ParseUnaryOperatorExpression();
    if (unaryOperatorExpression is not null)
    {
      return unaryOperatorExpression;
    }

    _position = startPosition;

    return null;
  }
}