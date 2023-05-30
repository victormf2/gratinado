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
          new AssignmentOperator(new EqualsToken()),
          new BinaryOperatorExpression(
            new BinaryOperatorExpression(
              new LiteralExpression(new NumberToken("2")),
              new AdditionOperator(new PlusToken()),
              new LiteralExpression(new NumberToken("4"))
            ),
            new AssignmentOperator(new EqualsToken()),
            new BinaryOperatorExpression(
              new BinaryOperatorExpression(
                new LiteralExpression(new NumberToken("3")),
                new AdditionOperator(new PlusToken()),
                new LiteralExpression(new NumberToken("5"))
              ),
              new AssignmentOperator(new EqualsToken()),
              new LiteralExpression(new NumberToken("6"))
            )
          )
        )
      ).Should().BeTrue();
    }

    [Fact]
    public void ShouldRespectAssociativityWithPrecedence()
    {
      var code = "1 ?? 2 = 3 ?? 4 = 5 = 6 ?? 7 ?? 8";
      var parser = new Parser(new Lexer(code));
      parser.Parse();
      var expression = parser.Expressions[0];

      expression.EqualsIgnorePosition(
        new BinaryOperatorExpression(
          new BinaryOperatorExpression(
            new LiteralExpression(new NumberToken("1")),
            new NullCoalescingOperator(new DoubleQuestionMarkToken()),
            new LiteralExpression(new NumberToken("2"))
          ),
          new AssignmentOperator(new EqualsToken()),
          new BinaryOperatorExpression(
            new BinaryOperatorExpression(
              new LiteralExpression(new NumberToken("3")),
              new NullCoalescingOperator(new DoubleQuestionMarkToken()),
              new LiteralExpression(new NumberToken("4"))
            ),
          new AssignmentOperator(new EqualsToken()),
            new BinaryOperatorExpression(
              new LiteralExpression(new NumberToken("5")),
              new AssignmentOperator(new EqualsToken()),
              new BinaryOperatorExpression(
                new LiteralExpression(new NumberToken("6")),
                new NullCoalescingOperator(new DoubleQuestionMarkToken()),
                new BinaryOperatorExpression(
                  new LiteralExpression(new NumberToken("7")),
                  new NullCoalescingOperator(new DoubleQuestionMarkToken()),
                  new LiteralExpression(new NumberToken("8"))
                )
              )
            )
          )
        )
      ).Should().BeTrue();
    }
  }
}