namespace Gratinado.Compiler
{
  public static class TokenExtensions
  {
    public static bool IsLiteralToken(this SyntaxToken token)
    {
      return token is NumberToken or StringToken;
    }
    public static bool IsBinaryOperatorToken(this SyntaxToken token)
    {
      return token is LowerThanOrEqualToken or
        LowerThanToken or
        GreaterThanOrEqualToken or
        GreaterThanToken or
        ExclamationEqualsToken or
        DoubleEqualsToken or
        EqualsToken or
        PlusToken or
        MinusToken or
        AsteriskToken or
        ForwardSlashToken or
        QuestionForwardSlashToken;
    }
    public static bool IsUnaryOperatorToken(this SyntaxToken token)
    {
      return token is PlusToken or MinusToken or ExclamationMarkToken;
    }
  }
  public class InvalidToken : SyntaxToken
  {
    public InvalidToken(string text) : this(-1 ,text) {}
    public InvalidToken(int position, string text) : base(position, text)
    {
    }
  }

  public class EOFToken : SyntaxToken
  {
    public EOFToken() : this(-1) {}
    public EOFToken(int position) : base(position, "")
    {
    }
  }

  public class OpenParenthesisToken : SyntaxToken
  {
    public OpenParenthesisToken() : this(-1) {}
    public OpenParenthesisToken(int position) : base(position, "(")
    {
    }
  }

  public class CloseParenthesisToken : SyntaxToken
  {
    public CloseParenthesisToken() : this(-1) {}
    public CloseParenthesisToken(int position) : base(position, ")")
    {
    }
  }

  public class OpenCurlyBracketsToken : SyntaxToken
  {
    public OpenCurlyBracketsToken() : this(-1) {}
    public OpenCurlyBracketsToken(int position) : base(position, "{")
    {
    }
  }


  public class CloseCurlyBracketsToken : SyntaxToken
  {
    public CloseCurlyBracketsToken() : this(-1) {}
    public CloseCurlyBracketsToken(int position) : base(position, "}")
    {
    }
  }

  public class OpenSquareBracketsToken : SyntaxToken
  {
    public OpenSquareBracketsToken() : this(-1) {}
    public OpenSquareBracketsToken(int position) : base(position, "{")
    {
    }
  }


  public class CloseSquareBracketsToken : SyntaxToken
  {
    public CloseSquareBracketsToken() : this(-1) {}
    public CloseSquareBracketsToken(int position) : base(position, "}")
    {
    }
  }
  public class NumberToken : SyntaxToken
  {
    public NumberToken(string text) : this(-1 ,text) {}
    public NumberToken(int position, string text) : base(position, text)
    {
    }
  }
  public class StringToken : SyntaxToken
  {
    public StringToken(string text) : this(-1 ,text) {}
    public StringToken(int position, string text) : base(position, text)
    {
    }
  }
  public class KeywordToken : SyntaxToken
  {
    public KeywordToken(string text) : this(-1 ,text) {}
    public KeywordToken(int position, string text) : base(position, text)
    {
    }
  }
  public class PlusToken : SyntaxToken
  {
    public PlusToken() : this(-1) {}
    public PlusToken(int position) : base(position, "+")
    {
    }
  }
  public class MinusToken : SyntaxToken
  {
    public MinusToken() : this(-1) {}
    public MinusToken(int position) : base(position, "-")
    {
    }
  }
  public class AsteriskToken : SyntaxToken
  {
    public AsteriskToken() : this(-1) {}
    public AsteriskToken(int position) : base(position, "*")
    {
    }
  }
  public class ForwardSlashToken : SyntaxToken
  {
    public ForwardSlashToken() : this(-1) {}
    public ForwardSlashToken(int position) : base(position, "/")
    {
    }
  }
  public class QuestionForwardSlashToken : SyntaxToken
  {
    public QuestionForwardSlashToken() : this(-1) {}
    public QuestionForwardSlashToken(int position) : base(position, "?/")
    {
    }
  }

  public class QuestionMarkToken : SyntaxToken
  {
    public QuestionMarkToken() : this(-1) {}
    public QuestionMarkToken(int position) : base(position, "?")
    {
    }
  }

  public class ExclamationMarkToken : SyntaxToken
  {
    public ExclamationMarkToken() : this(-1) {}
    public ExclamationMarkToken(int position) : base(position, "!")
    {
    }
  }

  public class EqualsToken : SyntaxToken
  {
    public EqualsToken() : this(-1) {}
    public EqualsToken(int position) : base(position, "=")
    {
    }
  }
  public class DoubleEqualsToken : SyntaxToken
  {
    public DoubleEqualsToken() : this(-1) {}
    public DoubleEqualsToken(int position) : base(position, "==")
    {
    }
  }
  public class ExclamationEqualsToken : SyntaxToken
  {
    public ExclamationEqualsToken() : this(-1) {}
    public ExclamationEqualsToken(int position) : base(position, "!=")
    {
    }
  }
  public class GreaterThanToken : SyntaxToken
  {
    public GreaterThanToken() : this(-1) {}
    public GreaterThanToken(int position) : base(position, ">")
    {
    }
  }
  public class GreaterThanOrEqualToken : SyntaxToken
  {
    public GreaterThanOrEqualToken() : this(-1) {}
    public GreaterThanOrEqualToken(int position) : base(position, ">=")
    {
    }
  }
  public class LowerThanToken : SyntaxToken
  {
    public LowerThanToken() : this(-1) {}
    public LowerThanToken(int position) : base(position, "<")
    {
    }
  }
  public class LowerThanOrEqualToken : SyntaxToken
  {
    public LowerThanOrEqualToken() : this(-1) {}
    public LowerThanOrEqualToken(int position) : base(position, "<=")
    {
    }
  }

}