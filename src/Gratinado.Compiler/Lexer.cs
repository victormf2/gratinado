using System.Collections;

namespace Gratinado.Compiler
{
  public interface ISyntaxNode
  {
    List<ISyntaxNode> Children { get; }
  }
  public interface IValueExpression : ISyntaxNode 
  {
    Prescedences Prescedence { get; }
  }
  public enum Prescedences
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
  public interface IOperator : ISyntaxNode {
    Prescedences Prescedence { get; }
  }
  public abstract class SyntaxToken : ISyntaxNode
  {
    public int Position { get; }
    public string Text { get; }

    public SyntaxToken(int position, string text)
    {
      Position = position;
      Text = text;
    }
    public List<ISyntaxNode> Children => new();

    public override string ToString()
    {
      return Text;
    }
  }

  public class InvalidToken : SyntaxToken
  {
    public InvalidToken(int position, string text) : base(position, text)
    {
    }
  }

  public class EOF : SyntaxToken
  {
    public EOF(int position) : base(position, "")
    {
    }
  }

  public class OpenParenthesis : SyntaxToken
  {
    public OpenParenthesis(int position) : base(position, "(")
    {
    }
  }

  /**
   interface created just to overcome
  */
  public interface ICloseParenthesis : ISyntaxNode {}
  public class CloseParenthesis : SyntaxToken, ICloseParenthesis
  {
    public CloseParenthesis(int position) : base(position, ")")
    {
    }
  }

  public static class CloseParenthesisExtensions
  {
    public static bool IsValid(this ICloseParenthesis closeParenthesisExpression, out CloseParenthesis closeParenthesis)
    {
      closeParenthesis = (closeParenthesisExpression as CloseParenthesis)!;
      return closeParenthesis != null;
    }
  }

  public class OpenBlock : SyntaxToken
  {
    public OpenBlock(int position) : base(position, "{")
    {
    }
  }

  public interface ICloseBlock : ISyntaxNode {}

  public class CloseBlock : SyntaxToken, ICloseBlock
  {
    public CloseBlock(int position) : base(position, "}")
    {
    }
  }

  public static class CloseBlockExtensions
  {
    public static bool IsValid(this ICloseBlock closeParenthesisExpression, out CloseBlock closeBlock)
    {
      closeBlock = (closeParenthesisExpression as CloseBlock)!;
      return closeBlock != null;
    }
  }

  public abstract class LiteralExpression : SyntaxToken, IValueExpression
  {
    protected LiteralExpression(int position, string text) : base(position, text)
    {
    }

    public Prescedences Prescedence => Prescedences.Literals;
  }

  public class NumberToken : LiteralExpression
  {
    public NumberToken(int position, string text) : base(position, text)
    {
    }
  }
  public class PlusSign : SyntaxToken, IOperator
  {
    public PlusSign(int position) : base(position, "+")
    {
    }

    public Prescedences Prescedence => Prescedences.Additive;
  }
  public class MinusSign : SyntaxToken, IOperator
  {
    public MinusSign(int position) : base(position, "-")
    {
    }

    public Prescedences Prescedence => Prescedences.Additive;
  }
  public class TimesSign : SyntaxToken, IOperator
  {
    public TimesSign(int position) : base(position, "*")
    {
    }

    public Prescedences Prescedence => Prescedences.Multiplicative;
  }
  public class DivisorSign : SyntaxToken, IOperator
  {
    public DivisorSign(int position) : base(position, "/")
    {
    }
    public Prescedences Prescedence => Prescedences.Multiplicative;
  }
  public class SafeDivisorSign : SyntaxToken, IOperator
  {
    public SafeDivisorSign(int position) : base(position, "?/")
    {
    }
    public Prescedences Prescedence => Prescedences.Multiplicative;
  }

  public class QuestionMark : SyntaxToken
  {
    public QuestionMark(int position) : base(position, "?")
    {
    }
  }

  public class Lexer : IEnumerable<SyntaxToken>
  {
    public string Text { get; }
    public List<IError> Errors { get; } = new();
    public Lexer(string text)
    {
      Text = text;
    }

    private int _position;
    private char CurrentChar => _position >= Text.Length ? '\0' : Text[_position];

    private int Next()
    {
      return ++_position;
    }

    private NumberToken ReadNumber()
    {
      int start = _position;
      while (char.IsDigit(CurrentChar))
      {
        _ = Next();
      }
      int numberTextLength = _position - start;
      string numberText = Text.Substring(start, numberTextLength);
      return new NumberToken(start, numberText);
    }

    private void SkipWhitespaces()
    {
      while (char.IsWhiteSpace(CurrentChar))
      {
        _ = Next();
      }
    }

    private SyntaxToken ReadSingleChar(Func<int, SyntaxToken> createToken)
    {
      int start = _position;
      _ = Next();
      return createToken(start);
    }

    public SyntaxToken ReadNextToken()
    {
      if (_position >= Text.Length)
      {
        return new EOF(_position);
      }

      SkipWhitespaces();

      if (char.IsDigit(CurrentChar))
      {
        return ReadNumber();
      }

      if (CurrentChar == '+')
      {
        return ReadSingleChar((position) => new PlusSign(position));
      }

      if (CurrentChar == '-')
      {
        return ReadSingleChar((position) => new MinusSign(position));
      }

      if (CurrentChar == '*')
      {
        return ReadSingleChar((position) => new TimesSign(position));
      }

      if (CurrentChar == '/')
      {
        return ReadSingleChar((position) => new DivisorSign(position));
      }

      if (CurrentChar == '(')
      {
        return ReadSingleChar((position) => new OpenParenthesis(position));
      }

      if (CurrentChar == ')')
      {
        return ReadSingleChar((position) => new CloseParenthesis(position));
      }

      if (CurrentChar == '{')
      {
        return ReadSingleChar((position) => new OpenBlock(position));
      }

      if (CurrentChar == '}')
      {
        return ReadSingleChar((position) => new CloseBlock(position));
      }

      if (CurrentChar == '?')
      {
        int start = _position;
        _ = Next();
        if (CurrentChar == '/')
        {
          _ = Next();
          return new SafeDivisorSign(start);
        }
        return new QuestionMark(start);
      }

      int invalidTokenPosition = _position;
      _ = Next();
      var invalidToken = new InvalidToken(invalidTokenPosition, Text.Substring(invalidTokenPosition, 1));
      Errors.Add(new InvalidTokenError(invalidToken));
      return invalidToken;
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
      if (Current is EOF) {
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
