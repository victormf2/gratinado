using Gratinado.Compiler;

namespace Gratinado.Tests
{
  public class UnitTest1
  {
    [Fact]
    public void ShouldParseUnaryOperators()
    {
      var codes = new [] { "+1", "-1", "!1" };
      var expressions = codes.Select(code => {
        var parser = new Parser(new Lexer(code));
        parser.Parse();
        return parser.Expressions[0];
      }).ToList();

      expressions.Should().HaveCount(3);
      expressions.Should().AllBeAssignableTo<UnaryOperatorExpression>();
    }

    [Fact]
    public void ShouldRespectAssociativity()
    {
      var code = "1 = 2 + 4 = 3 + 5 = 6";
      var parser = new Parser(new Lexer(code));
      parser.Parse();
      var expression = parser.Expressions[0];

      expression.EqualsIgnorePosition(
        new BinaryOperatorExpression(
          new LiteralExpression(new NumberToken("1")),
          new EqualsToken(),
          new BinaryOperatorExpression(
            new BinaryOperatorExpression(
              new LiteralExpression(new NumberToken("2")),
              new PlusToken(),
              new LiteralExpression(new NumberToken("4"))
            ),
            new EqualsToken(),
            new BinaryOperatorExpression(
              new BinaryOperatorExpression(
                new LiteralExpression(new NumberToken("3")),
                new PlusToken(),
                new LiteralExpression(new NumberToken("5"))
              ),
              new EqualsToken(),
              new LiteralExpression(new NumberToken("6"))
            )
          )
        )
      ).Should().BeTrue();
    }
  }
}