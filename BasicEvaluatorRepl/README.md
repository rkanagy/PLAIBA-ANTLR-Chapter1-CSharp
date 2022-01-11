# Basic Evaluator Read-Eval-Print Loop source code

The function main is located in the file [BasicEvaluatorRepl.cs](BasicEvaluatorRepl.cs).
This file contains the main Read-Eval-Print Loop which gets input from the user,
that consists of a set of lines containing one expression or function definition.
The expressions or function definitions entered by the user are then parsed and
evaluated.  See the methods named **parseDefinition**, **parseExpression**, in the Parser.cs file in the BasicEvaluatorInterpreter project to see how to use ANTLR to parse this language from the
string entered by the user at the REPL prompt.

Expressions are evaluated to a single Integer value and displayed as
output.  Function definitions are parsed and then added to a list of function
definitions available to subsequent expressions entered, and its name is displayed
as output.

To run the evaluator from the Rider terminal, run the following command from
the **bin/Debug/net6.0** folder, after building the project from
within the IDE:
```
./BasicEvaluator 
```

This will then display a prompt for the user to enter a function definition or
an expression to be evaluated.  The file [test01.lp](test01.lp) contains a set
of expressions and function definitions that can be entered at the prompt to
test the Interpreter.
