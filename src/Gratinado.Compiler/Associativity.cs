namespace Gratinado.Compiler
{
  public enum Associativity
  {
    FromLeftToRight,
    FromRightToLeft,
  }

  public static class AssociativityExtensions
  {
    public static Associativity GetAssociativity(this ISyntaxNode node)
    {
      return node switch
      {
        EqualsToken => Associativity.FromRightToLeft,
        _ => Associativity.FromLeftToRight,
      };
    }
  }
}