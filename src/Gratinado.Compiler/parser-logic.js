function parse() {
  const expressions = []
  let currentExpression = parseExpression()
  while (!(currentExpression instanceof EOF)) {

    if (currentExpression instanceof IValueExpression) {
      expressions.push(currentExpression)
    } else {
      errors.push(
        new ExpectedExpressionError({
          expression: currentExpression
        })
      )

      expressions.push(new InvalidExpression({ expression: currentExpression }))
    }
  }

  return expressions
}

function parseExpression() {
  let currentParsedToken = readNextToken()
  if (currentParsedToken instanceof EOF) {
    return currentParsedToken
  }

  let accumulatedExpression = null
  let operator = null

  while (true) {

    if (currentParsedToken instanceof OpenParenthesis) {
      accumulatedExpression = parseOperatorExpression({
        left: accumulatedExpression,
        right: parseParenthesisExpression({
          openParenthesis: currentParsedToken
        }),
        operator
      })
    } else if (currentParsedToken instanceof CloseParenthesis) {
      return currentParsedToken
    } else if (currentParsedToken instanceof OpenBlock) {
      accumulatedExpression = parseOperatorExpression({
        left: accumulatedExpression,
        right: parseBlockExpression({
          openBlock: currentParsedToken
        }),
        operator
      })
    } else if (currentParsedToken instanceof CloseBlock) { 
      return currentParsedToken
    } else if (currentParsedToken instanceof IValueExpression) {
      accumulatedExpression = parseOperatorExpression({
        left: accumulatedExpression,
        right: currentParsedToken,
        operator
      })
    } else {
      errors.push(
        new InvalidTokenError({
          token: currentParsedToken
        })
      )
      currentParsedToken = readNextToken()
      continue;
    }

    const nextParsedToken = readNextToken()

    if (!(nextParsedToken instanceof IOperator)) {
      position -= 1
      return accumulatedExpression;
    }
    operator = nextParsedToken
    currentParsedToken = readNextToken()
  }
}

function parseParenthesisExpression({
  openParenthesis
}) {
  const expression = parseExpression()

  if (expression instanceof CloseParenthesis) {
    errors.push(new ExpectedExpressionError({
      expression
    }))

    return new ParenthesisExpression({
      openParenthesis,
      expression: new EmptyValueExpression(),
      closeParenthesis: expression
    })
  }

  const isValueExpression = expression instanceof IValueExpression
  if (!isValueExpression) {
    errors.push(
      new ExpectedExpressionError({
        expression,
      })
    )
  }

  const nextParsedToken = readNextToken()
  if (nextParsedToken instanceof CloseParenthesis) {
    return new ParenthesisExpression({
      openParenthesis,
      expression,
      closeParenthesis: nextParsedToken
    })
  }

  errors.push(
    new ExpectedCloseParenthesisError({
      openParenthesis,
      expression,
      nextParsedToken
    })
  )
  position -= 1
  return new ParenthesisExpression({
    openParenthesis,
    expression,
    closeParenthesis: new InvalidCloseParenthesis({ expression: nextParsedToken })
  })
}

function parseBlockExpression({
  openBlock
}) {
  let expressions = []
  let currentExpression = parseExpression()
  while (currentExpression instanceof IValueExpression) {
    expressions.push(currentExpression)
    currentExpression = parseExpression()
  }
  if (currentExpression instanceof CloseBlock) {
    return new Block({
      openBlock,
      expressions,
      closeBlock: currentExpression
    })
  }

  errors.push(
    new ExpectedCloseBlockError({
      openBlock,
      expressions,
      nextParsedExpression: currentExpression
    })
  )

  position -= 1
  return new Block({
    openBlock,
    expressions,
    closeBlock: new InvalidCloseBlock({ expression: currentExpression })
  })

  
}

function readNextToken() {
  const nextToken = tokens[position];
  position++
  return nextToken
}

// TODO todas as classes de ParseResult 
//  - herdam o isEOF de seus parâmetros do construtor
//  - herdam errors de seus parâmetros do construtor
//  - herdam quantidade de tokens lidos de seus parâmetros do construtor

