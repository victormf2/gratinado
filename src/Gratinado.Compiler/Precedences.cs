namespace Gratinado.Compiler
{
  public enum Precedences
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
    RelationalAndTypeTesting, // < > <= etc
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
}