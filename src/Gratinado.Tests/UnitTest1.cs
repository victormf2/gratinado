using Gratinado.Compiler;

namespace Gratinado.Tests
{
  public class UnitTest1
  {
    [Fact]
    public void Lexer_Test1()
    {
      Lexer lexer = new("(1 ?/ \n0) + 345 / 4 * 5");
      List<SyntaxToken> tokens = lexer.ToList();

      _ = tokens.Should().BeEquivalentTo(new SyntaxToken[] {
        new OpenParenthesis(0),
        new NumberToken(1, "1"),
        new SafeDivisorSign(3),
        new NumberToken(7, "0"),
        new CloseParenthesis(8),
        new PlusSign(10),
        new NumberToken(12, "345"),
        new DivisorSign(16),
        new NumberToken(18, "4"),
        new TimesSign(20),
        new NumberToken(22, "5")
      });
    }

    [Fact]
    public void Parser_Test0()
    { /*
      1
      (1 + 2)
      (1 + (2 * 3))
      ((1 + (2 * 3)) + 4)
      ((1 + (2 * 3)) + (4 * 5))

      */
      var lexer = new Lexer("1 + 2 * 3 + 4 * 5 / 6 + 7 / 12 * 5");
      var parser = new Parser(lexer);

      parser.Parse();
      var expressions = parser.Expressions;
      expressions[0].ToString().Should().Be("(((1 + (2 * 3)) + ((4 * 5) / 6)) + ((7 / 12) * 5))");
    }

    [Fact]
    public void Parser_Test1()
    {
      var lexer = new Lexer("1 + 2 + 3");
      var parser = new Parser(lexer);

      parser.Parse();
      var expressions = parser.Expressions;
      var errors = parser.Errors;

      expressions.Should().NotBeNull();

      expressions.Should().HaveCount(1);

      expressions[0].ToString().Should().Be("((1 + 2) + 3)");

      errors.Should().BeEmpty();
    }

    [Fact]
    public void Parser_Test2()
    {
      var lexer = new Lexer("1 + 2 ?/ (3 + 5) + { 12 { 45 + 5 } }");
      var parser = new Parser(lexer);

      parser.Parse();
      var expressions = parser.Expressions;
      var errors = parser.Errors;

      expressions.Should().NotBeNull();

      expressions.Should().HaveCount(1);

      var expected = "1 + 2 ?/ (3 + 5) + {\n  12\n  {\n    45 + 5\n  }\n}";
      var actual = expressions[0].ToString();
      actual.Should().Be(expected);

      errors.Should().BeEmpty();
    }

    [Fact]
    public void Parser_Test3()
    {
      var lexer = new Lexer("?");
      var parser = new Parser(lexer);

      parser.Parse();
      var expressions = parser.Expressions;
      var errors = parser.Errors;

      expressions.Should().NotBeNull();

      expressions.Should().HaveCount(1);

      expressions[0].ToString().Should().Be("?");
      expressions[0].Should().BeOfType<InvalidExpression>().And.BeEquivalentTo(
        new InvalidExpression(new QuestionMark(0))
      );

      errors.Should().HaveCount(1);
      errors[0].Should().BeOfType<InvalidTokenError>().And.BeEquivalentTo(
        new InvalidTokenError(new QuestionMark(0))
      );
    }

    [Fact]
    public void Parser_Test4()
    {
      var lexer = new Lexer("(+ 12 + 45 * (64 / 36] + 37)");
      var parser = new Parser(lexer);

      parser.Parse();
      var expressions = parser.Expressions;
      var errors = parser.Errors;

      errors.Should().HaveCount(3);
    }
  }
}