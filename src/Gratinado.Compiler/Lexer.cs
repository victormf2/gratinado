using System.Collections;

namespace Gratinado.Compiler
{
  public struct FilePosition
  {
    public int column;
    public int line;
    public FilePosition()
    {
      column = 0;
      line = 1;
    }
  }
  public interface ISyntaxNode
  {
    int Start { get; }
    int End { get; }
    IEnumerable<ISyntaxNode> Children { get; }
    bool EqualsIgnorePosition(ISyntaxNode? other);
  }
  public abstract class SyntaxToken : ISyntaxNode
  {
    public int Start { get; }

    public int End { get; }
    public string Text { get; }
    public FilePosition Position { get; }

    public SyntaxToken(int textPosition, FilePosition filePosition, string text)
    {
      Start = textPosition;
      Text = text;
      End = Start + Text.Length;
      Position = filePosition;
    }
    public IEnumerable<ISyntaxNode> Children => Enumerable.Empty<ISyntaxNode>();

    public override string ToString()
    {
      return Text;
    }

    public bool EqualsIgnorePosition(ISyntaxNode? node)
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

    private int _textPosition;
    private FilePosition _filePosition = new();
    private void Read(int length = 1)
    {
      _filePosition.column += length;
      _textPosition += length;
    }

    private char Peek(int offset)
    {
      return _textPosition >= Text.Length ? '\0' : Text[_textPosition + offset];
    }

    private char CurrentChar => Peek(0);

    private NumberToken ReadNumber()
    {
      int startTextPosition = _textPosition;
      FilePosition startFilePosition = _filePosition;
      while (char.IsDigit(CurrentChar))
      {
        Read();
      }
      int numberTextLength = _textPosition - startTextPosition;
      string numberText = Text.Substring(startTextPosition, numberTextLength);
      return new NumberToken(startTextPosition, startFilePosition, numberText);
    }

    private KeywordToken ReadKeyword()
    {
      int startTextPosition = _textPosition;
      FilePosition startFilePosition = _filePosition;
      while (char.IsLetterOrDigit(CurrentChar) || CurrentChar == '_')
      {
        Read();
      }
      int keywordTextLength = _textPosition - startTextPosition;
      string keywordText = Text.Substring(startTextPosition, keywordTextLength);
      return new KeywordToken(startTextPosition, startFilePosition, keywordText);
    }

    private void SkipWhitespaces()
    {
      while (char.IsWhiteSpace(CurrentChar))
      {
        if (CurrentChar == '\n')
        {
          _filePosition.line++;
          _filePosition.column = -1;
        }
        Read();
      }
    }

    public SyntaxToken ReadNextToken()
    {
      SyntaxToken? nextToken;
      do
      {
        SkipWhitespaces();

        nextToken = CurrentChar switch
        {
          '\0' => new EOFToken(_textPosition, _filePosition),
          '+' => new PlusToken(_textPosition, _filePosition),
          '-' => new MinusToken(_textPosition, _filePosition),
          '*' => new AsteriskToken(_textPosition, _filePosition),
          '/' => new ForwardSlashToken(_textPosition, _filePosition),
          '(' => new OpenParenthesisToken(_textPosition, _filePosition),
          ')' => new CloseParenthesisToken(_textPosition, _filePosition),
          '{' => new OpenCurlyBracketsToken(_textPosition, _filePosition),
          '}' => new CloseCurlyBracketsToken(_textPosition, _filePosition),
          ':' => new ColonToken(_textPosition, _filePosition),
          ',' => new CommaToken(_textPosition, _filePosition),
          '?' => Peek(1) switch
          {
            '/' => new QuestionForwardSlashToken(_textPosition, _filePosition),
            '?' => new DoubleQuestionMarkToken(_textPosition, _filePosition),
            _ => null,
          },
          '!' => Peek(1) switch
          {
            '=' => new ExclamationEqualsToken(_textPosition, _filePosition),
            _ => new ExclamationMarkToken(_textPosition, _filePosition)
          },
          '=' => Peek(1) switch
          {
            '=' => new DoubleEqualsToken(_textPosition, _filePosition),
            _ => new EqualsToken(_textPosition, _filePosition)
          },
          '<' => Peek(1) switch
          {
            '=' => new LowerThanOrEqualToken(_textPosition, _filePosition),
            _ => new LowerThanToken(_textPosition, _filePosition)
          },
          '>' => Peek(1) switch
          {
            '=' => new GreaterThanOrEqualToken(_textPosition, _filePosition),
            _ => new GreaterThanToken(_textPosition, _filePosition)
          },
          _ when char.IsDigit(CurrentChar) => ReadNumber(),
          _ when char.IsLetter(CurrentChar) => ReadKeyword(),
          _ => null
        };

        if (nextToken is not null)
        {
          Read(nextToken.Text.Length);
        }
        else
        {
          var invalidToken = new InvalidToken(
            _textPosition,
            _filePosition,
            Text.Substring(_textPosition, 1)
          );
          Diagnostics.Add(new InvalidTokenDiagnostic(invalidToken));
          Read();
        }
      }
      while(nextToken is null);

      return nextToken;
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

  public sealed class LexerEnumerator : IEnumerator<SyntaxToken>
  {
    public Lexer Lexer { get; }
    public LexerEnumerator(Lexer lexer)
    {
      Lexer = lexer;

    }
    public SyntaxToken Current { get; private set; } = new InvalidToken("");

    object IEnumerator.Current => throw new NotImplementedException();

    public void Dispose() {}

    public bool MoveNext()
    {
      Current = Lexer.ReadNextToken();
      return Current is not EOFToken;
    }

    public void Reset()
    {
      throw new NotImplementedException();
    }
  }
}
