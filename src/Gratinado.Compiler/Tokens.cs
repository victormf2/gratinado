namespace Gratinado.Compiler
{
  public class InvalidToken : SyntaxToken
  {
    public 
    InvalidToken(string text) : this(-1, default, text) {}
    public InvalidToken(int textPosition, FilePosition filePosition, string text)
      : base(textPosition, filePosition, text)
    {
    }
  }

  public class EOFToken : SyntaxToken
  {
    public EOFToken() : this(-1, default) {}
    public EOFToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "EOF")
    {
    }
  }

  public class OpenParenthesisToken : SyntaxToken
  {
    public OpenParenthesisToken() : this(-1, default) {}
    public OpenParenthesisToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "(")
    {
    }
  }

  public class CloseParenthesisToken : SyntaxToken
  {
    public CloseParenthesisToken() : this(-1, default) {}
    public CloseParenthesisToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, ")")
    {
    }
  }

  public class OpenCurlyBracketsToken : SyntaxToken
  {
    public OpenCurlyBracketsToken() : this(-1, default) {}
    public OpenCurlyBracketsToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "{")
    {
    }
  }


  public class CloseCurlyBracketsToken : SyntaxToken
  {
    public CloseCurlyBracketsToken() : this(-1, default) {}
    public CloseCurlyBracketsToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "}")
    {
    }
  }

  public class NumberToken : SyntaxToken
  {
    public NumberToken(string text) : this(-1, default, text) {}
    public NumberToken(int textPosition, FilePosition filePosition, string text)
      : base(textPosition, filePosition, text)
    {
    }
  }
  public class StringToken : SyntaxToken
  {
    public StringToken(string text) : this(-1, default, text) {}
    public StringToken(int textPosition, FilePosition filePosition, string text)
      : base(textPosition, filePosition, text)
    {
    }
  }
  public class KeywordToken : SyntaxToken
  {
    public KeywordToken(string text) : this(-1, default, text) {}
    public KeywordToken(int textPosition, FilePosition filePosition, string text)
      : base(textPosition, filePosition, text)
    {
    }
  }
  public class PlusToken : SyntaxToken
  {
    public PlusToken() : this(-1, default) {}
    public PlusToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "+")
    {
    }
  }
  public class MinusToken : SyntaxToken
  {
    public MinusToken() : this(-1, default) {}
    public MinusToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "-")
    {
    }
  }
  public class AsteriskToken : SyntaxToken
  {
    public AsteriskToken() : this(-1, default) {}
    public AsteriskToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "*")
    {
    }
  }
  public class ForwardSlashToken : SyntaxToken
  {
    public ForwardSlashToken() : this(-1, default) {}
    public ForwardSlashToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "/")
    {
    }
  }
  public class QuestionForwardSlashToken : SyntaxToken
  {
    public QuestionForwardSlashToken() : this(-1, default) {}
    public QuestionForwardSlashToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "?/")
    {
    }
  }
  public class DoubleQuestionMarkToken : SyntaxToken
  {

    public DoubleQuestionMarkToken() : this(-1, default) {}
    public DoubleQuestionMarkToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "??")
    {
    }
  }

  public class ExclamationMarkToken : SyntaxToken
  {
    public ExclamationMarkToken() : this(-1, default) {}
    public ExclamationMarkToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "!")
    {
    }
  }

  public class EqualsToken : SyntaxToken
  {
    public EqualsToken() : this(-1, default) {}
    public EqualsToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "=")
    {
    }
  }
  public class DoubleEqualsToken : SyntaxToken
  {
    public DoubleEqualsToken() : this(-1, default) {}
    public DoubleEqualsToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "==")
    {
    }
  }
  public class ExclamationEqualsToken : SyntaxToken
  {
    public ExclamationEqualsToken() : this(-1, default) {}
    public ExclamationEqualsToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "!=")
    {
    }
  }
  public class ColonToken : SyntaxToken
  {
    public ColonToken() : this(-1, default) {}
    public ColonToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, ":")
    {
    }
  }
  public class CommaToken : SyntaxToken
  {
    public CommaToken() : this(-1, default) {}
    public CommaToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, ",")
    {
    }
  }
  public class GreaterThanToken : SyntaxToken
  {
    public GreaterThanToken() : this(-1, default) {}
    public GreaterThanToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, ">")
    {
    }
  }
  public class GreaterThanOrEqualToken : SyntaxToken
  {
    public GreaterThanOrEqualToken() : this(-1, default) {}
    public GreaterThanOrEqualToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, ">=")
    {
    }
  }
  public class LowerThanToken : SyntaxToken
  {
    public LowerThanToken() : this(-1, default) {}
    public LowerThanToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "<")
    {
    }
  }
  public class LowerThanOrEqualToken : SyntaxToken
  {
    public LowerThanOrEqualToken() : this(-1, default) {}
    public LowerThanOrEqualToken(int textPosition, FilePosition filePosition)
      : base(textPosition, filePosition, "<=")
    {
    }
  }

}