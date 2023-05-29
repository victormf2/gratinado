using System.Collections;

namespace Gratinado.Compiler
{
  public interface ISyntaxNode
  {
    int Start { get; }
    int End { get; }
    List<ISyntaxNode> Children { get; }
    bool EqualsIgnorePosition(ISyntaxNode node);
  }
  public abstract class SyntaxToken : ISyntaxNode
  {
    public int Start { get; }
    public string Text { get; }

    public SyntaxToken(int position, string text)
    {
      Start = position;
      Text = text;
      End = Start + Text.Length;
    }
    public List<ISyntaxNode> Children => new();

    public int End { get; }

    public override string ToString()
    {
      return Text;
    }

    public bool EqualsIgnorePosition(ISyntaxNode node)
    {
      return node is SyntaxToken other && other.GetType() == GetType() && other.Text == Text;
    }
  }

  public class Lexer : IEnumerable<SyntaxToken>
  {
    public string Text { get; }
    public List<Diagnostic> Diagnostics { get; } = new();
    public Lexer(string text)
    {
      Text = text;
    }

    private int _position;
    private void NextChar()
    {
      _position++;
    }

    private char CurrentChar => _position >= Text.Length ? '\0' : Text[_position];

    private NumberToken ReadNumber()
    {
      int start = _position;
      while (char.IsDigit(CurrentChar))
      {
        _position++;
      }
      int numberTextLength = _position - start;
      string numberText = Text.Substring(start, numberTextLength);
      return new NumberToken(start, numberText);
    }

    private void SkipWhitespaces()
    {
      while (char.IsWhiteSpace(CurrentChar))
      {
        NextChar();
      }
    }

    public SyntaxToken ReadNextToken()
    {
      while (_position < Text.Length)
      {
        SkipWhitespaces();

        if (char.IsDigit(CurrentChar))
        {
          return ReadNumber();
        }

        if (CurrentChar == '+')
        {
          return new PlusToken(_position++);
        }

        if (CurrentChar == '-')
        {
          return new MinusToken(_position++);
        }

        if (CurrentChar == '*')
        {
          return new AsteriskToken(_position++);
        }

        if (CurrentChar == '/')
        {
          return new ForwardSlashToken(_position++);
        }

        if (CurrentChar == '(')
        {
          return new OpenParenthesisToken(_position++);
        }

        if (CurrentChar == ')')
        {
          return new CloseParenthesisToken(_position++);
        }

        if (CurrentChar == '{')
        {
          return new OpenCurlyBracketsToken(_position++);
        }

        if (CurrentChar == '}')
        {
          return new CloseCurlyBracketsToken(_position++);
        }

        if (CurrentChar == '?')
        {
          int start = _position++;
          if (CurrentChar == '/')
          {
            _position++;
            return new QuestionForwardSlashToken(start);
          }
          return new QuestionMarkToken(start);
        }

        if (CurrentChar == '!')
        {
          int start = _position++;
          if (CurrentChar == '=')
          {
            _position++;
            return new ExclamationEqualsToken(start);
          }
          return new ExclamationMarkToken(start);
        }

        if (CurrentChar == '=')
        {
          int start = _position++;
          if (CurrentChar == '=')
          {
            _position++;
            return new DoubleEqualsToken(start);
          }
          return new EqualsToken(start);
        }

        if (CurrentChar == '<')
        {
          int start = _position++;
          if (CurrentChar == '=')
          {
            _position++;
            return new LowerThanOrEqualToken(start);
          }
          return new LowerThanToken(start);
        }

        if (CurrentChar == '>')
        {
          int start = _position++;
          if (CurrentChar == '=')
          {
            _position++;
            return new GreaterThanOrEqualToken(start);
          }
          return new GreaterThanToken(start);
        }

        int invalidTokenPosition = _position;
        _position++;
        var invalidToken = new InvalidToken(invalidTokenPosition, Text.Substring(invalidTokenPosition, 1));
        Diagnostics.Add(new InvalidTokenDiagnostic(invalidToken));
        continue;
      }
      return new EOFToken(_position);
    }

    public IEnumerator<SyntaxToken> GetEnumerator()
    {
      return new LexerEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return new LexerEnumerator(this);
    }
  }

  public class LexerEnumerator : IEnumerator<SyntaxToken>
  {
    public Lexer Lexer { get; }
    public LexerEnumerator(Lexer lexer)
    {
      Lexer = lexer;

    }
    public SyntaxToken Current { get; private set; }

    object IEnumerator.Current => throw new NotImplementedException();

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
      if (Current is EOFToken) {
        return false;
      }
      Current = Lexer.ReadNextToken();
      return true;
    }

    public void Reset()
    {
      throw new NotImplementedException();
    }
  }
}
