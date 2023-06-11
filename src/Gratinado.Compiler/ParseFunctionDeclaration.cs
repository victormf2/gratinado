namespace Gratinado.Compiler;
public partial class Parser
{
  private TypeExpression? ParseType()
  {
    var typeIdentifier = ParseIdentifier();
    if (typeIdentifier is not null)
    {
      return new NamedType(typeIdentifier);
    }
    return null;
  }
  private Parameter ParseParameter(CommaToken? comma = null)
  {
    var identifier = ParseIdentifier();

    while (identifier is null)
    {
      var unexpectedToken = ReadNextToken();
      Diagnostics.Add(new ParameterDeclarationExpectedDiagnostic(unexpectedToken));
      identifier = ParseIdentifier();
    }

    TypeExpression? type = null;
    var colon = Match<ColonToken>();
    if (colon is not null)
    {
      type = ParseType();
      if (type is null)
      {
        Diagnostics.Add(new TypeExpectedDiagnostic(_position + 1));
      }
    }

    return new Parameter(comma, identifier, colon, type);
  }
  private ParametersList? ParseParametersList()
  {
    var openParenthesis = Match<OpenParenthesisToken>();
    if (openParenthesis is null)
    {
      return null;
    }
    var nextToken = Peek();
    var parameters = new List<Parameter>();
    if (nextToken is not (CloseParenthesisToken or OpenCurlyBracketsToken))
    {
      var firstParameter = ParseParameter();
      parameters.Add(firstParameter);
    }

    while (nextToken is not (CloseParenthesisToken or OpenCurlyBracketsToken))
    {
      var comma = Match<CommaToken>();
      if (comma is null)
      {
        Diagnostics.Add(new CommaExpectedDiagnostic(_position + 1));
      }
      var nextParameter = ParseParameter(comma);
      parameters.Add(nextParameter);
    }

    var closeParenthesis = Match<CloseParenthesisToken>();
    if (closeParenthesis is null)
    {
      Diagnostics.Add(new CloseParenthesisExpectedDiagnostic(_position + 1));
    }

    return new ParametersList(openParenthesis, parameters, closeParenthesis);
  }
  private Identifier? ParseIdentifier()
  {
    var keywordToken = Match<KeywordToken>();
    if (keywordToken is null)
    {
      return null;
    }
    return new Identifier(keywordToken);
  }
  private Expression? ParseFunctionDeclaration()
  {
    var functionKeywordToken = MatchKeyword("function");
    if (functionKeywordToken is null)
    {
      return null;
    }

    var identifier = ParseIdentifier();
    if (identifier is null)
    {
      Diagnostics.Add(new IdentifierExpectedDiagnostic(_position + 1));
    }

    var parametersList = ParseParametersList();
    if (parametersList is null)
    {
      Diagnostics.Add(new ParametersListExpectedDiagnostic(_position + 1));
    }

    var functionBody = ParseBlockExpression();
    if (functionBody is null)
    {
      Diagnostics.Add(new FunctionBodyExpectedDiagnostic(_position + 1));
    }

    return new FunctionDeclaration(
      functionKeywordToken,
      identifier,
      parametersList,
      functionBody
    );
  }
}