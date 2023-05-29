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
    public ExpressionExpectedDiagnostic(ISyntaxNode node) : this(node.Start, node.End) {}
    public ExpressionExpectedDiagnostic(int start, int end) : base(start, end, "Expression expected")
    {
    }
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
    public CloseParenthesisExpectedDiagnostic(ISyntaxNode node) : this(node.Start, node.End) {}
    public CloseParenthesisExpectedDiagnostic(int start, int end) : base(start, end, "Expression expected")
    {
    }
  }

  public class CloseCurlyBracketsExpectedDiagnostic : ErrorDiagnostic
  {
    public CloseCurlyBracketsExpectedDiagnostic(ISyntaxNode node) : this(node.Start, node.End) {}
    public CloseCurlyBracketsExpectedDiagnostic(int start, int end) : base(start, end, "Expression expected")
    {
    }
  }
}