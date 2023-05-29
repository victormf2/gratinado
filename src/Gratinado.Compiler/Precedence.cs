namespace Gratinado.Compiler
{
  public enum Precedence
  {
    Literals = 0,
    AssignmentAndLambdaDeclarations,
    ConditionalTernaryOperator,
    NullCoalescingOperator,
    ConditionalOR,
    ConditionalAND,
    BitwiseOR,
    BitwiseXOR,
    BitwiseAND,
    EqualityComparison,
    RelationalAndCasting, // < > <= etc
    BitShift,
    Additive,
    Multiplicative,
    SwitchExpressions,
    Unary,
    Primary,
    Declaration,
    Block,
    Parenthesis,
  }

  public static class PrecedenceExtesions
  {


    public static Precedence GetPrecedence(this ISyntaxNode node)
    {
      var n = node switch
      {
        EqualsToken => Precedence.AssignmentAndLambdaDeclarations,
        AsteriskToken or ForwardSlashToken or QuestionForwardSlashToken => Precedence.Multiplicative,
        PlusToken or MinusToken => Precedence.Additive,
        _ => Precedence.Literals,
      };
      return n;
    }
  }
}