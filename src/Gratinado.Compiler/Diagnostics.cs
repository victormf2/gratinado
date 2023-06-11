using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gratinado.Compiler
{
  public class Diagnostic
  {
    public int Start { get; }
    public int End { get; }
    public string Message { get; }

    public Diagnostic(int start, int end, string message)
    {
      Start = start;
      End = end;
      Message = message;
    }
  }

  public abstract class ErrorDiagnostic : Diagnostic
  {
    protected ErrorDiagnostic(int start, int end, string message) : base(start, end, message)
    {
    }
  }

  public class ExpressionExpectedDiagnostic : ErrorDiagnostic
  {
    public ExpressionExpectedDiagnostic(int position)
      : base(position, position, "Expression expected")
    {}
    public ExpressionExpectedDiagnostic(SyntaxToken unexpectedToken)
      : base(unexpectedToken.Start, unexpectedToken.End, "Expression expected")
    {}
  }

  public class InvalidTokenDiagnostic : ErrorDiagnostic
  {
    public InvalidTokenDiagnostic(InvalidToken token) : this(token.Start, token.End) {}
    public InvalidTokenDiagnostic(int start, int end) : base(start, end, "Expression expected")
    {
    }
  }

  public class CloseParenthesisExpectedDiagnostic : ErrorDiagnostic
  {
    public CloseParenthesisExpectedDiagnostic(int position)
      : base(position, position, "')' expected")
    {
    }
  }

  public class CloseCurlyBracketsExpectedDiagnostic : ErrorDiagnostic
  {
    public CloseCurlyBracketsExpectedDiagnostic(int position)
      : base(position, position, "'}' expected")
    {
    }
  }

  public class EOFExpectedDiagnostic : ErrorDiagnostic
  {
    public EOFExpectedDiagnostic(int position)
      : base(position, position, "EOF expected")
    {}
  }

  public class ParametersListExpectedDiagnostic : ErrorDiagnostic
  {
    public ParametersListExpectedDiagnostic(int position)
      : base(position, position, "( expected")
    {}
  }

  public class IdentifierExpectedDiagnostic : ErrorDiagnostic
  {
    public IdentifierExpectedDiagnostic(int position)
      : base(position, position, "Identifier expected")
    {}
  }

  public class FunctionBodyExpectedDiagnostic : ErrorDiagnostic
  {
    public FunctionBodyExpectedDiagnostic(int position)
      : base(position, position, "{ expected")
    {}
  }

  public class CommaExpectedDiagnostic : ErrorDiagnostic
  {
    public CommaExpectedDiagnostic(int position)
      : base(position, position, ", expected")
    {}
  }

  public class ParameterDeclarationExpectedDiagnostic : ErrorDiagnostic
  {
    public ParameterDeclarationExpectedDiagnostic(SyntaxToken unexpectedToken)
      : base(unexpectedToken.Start, unexpectedToken.End, "Parameter declaration expected")
    {}
  }

  public class TypeExpectedDiagnostic : ErrorDiagnostic
  {
    public TypeExpectedDiagnostic(int position)
      : base(position, position, "Type expected")
    {}
  }
}