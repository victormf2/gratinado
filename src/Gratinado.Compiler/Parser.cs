namespace Gratinado.Compiler
{
  public partial class Parser
  {
    public List<Diagnostic> Diagnostics { get; } = new();
    public List<Expression> Expressions = new();
    private readonly List<SyntaxToken> _tokens = new();
    private int _position = 0;
    public Lexer Lexer { get; set; }
    public Parser(Lexer lexer)
    {
      Lexer = lexer;
      _tokens = lexer.ToList();
    }

    public void Parse()
    {
      var currentExpression = ParseExpression();
      while (currentExpression is not null)
      {
        Expressions.Add(currentExpression);
        currentExpression = ParseExpression();
      }
    }

    private SyntaxToken ReadNextToken()
    {
      var nextToken = _tokens[_position];
      _position += 1;
      return nextToken;
    }

    private SyntaxToken Peek(int offest = 0)
    {
      return _tokens[_position + offest];
    }
  }
}