# Basic Evaluator Interpreter source code

The grammar for the programming language being implemented is in the file 
[BasicEvaluator.g4](BasicEvaluator.g4).

After any changes to the grammar file, the lexer and parser code needs to be 
rebuilt using the following command in Linux:

```
antlr4 -no-listener -visitor -Dlanguage=CSharp BasicEvaluator.g4
```

The command antlr4 is an alias in Linux set to:
```
java -jar /usr/local/lib/antlr-4.9.3-complete.jar
```

The antlr4 command will rebuild the Lexer and Parser, 
consisting of the following files:
- [BasicEvaluator.interp](BasicEvaluator.interp)
- [BasicEvaluator.tokens](BasicEvaluator.tokens)
- [BasicEvaluatorBaseVisitor.cs](BasicEvaluatorBaseVisitor.cs)
- [BasicEvaluatorLexer.cs](BasicEvaluatorLexer.cs)
- [BasicEvaluatorLexer.interp](BasicEvaluatorLexer.interp)
- [BasicEvaluatorLexer.tokens](BasicEvaluatorLexer.tokens)
- [BasicEvaluatorParser.cs](BasicEvaluatorParser.cs)
- [BasicEvaluatorVisitor.cs](BasicEvaluatorVisitor.cs)

The following files were created to implement the Interpreter:
- [BasicEvaluatorVisitorImpl.cs](BasicEvaluatorVisitorImpl.cs) - the 
implementation of the visitor which implements the main part of the Interpreter.  The class inherits from the auto-generated generic BasicEvaluatorBaseVisitor class.
- [BasicEvaluatorInfo.cs](BasicEvaluatorInfo.cs) - the class of the object that
is returned from each visitor function, consisting of either the name of the 
function definition or the value of the expression being evaluated 
- [Memory.cs](Memory.cs) - contains the collections of variable values for
both the global and local environments (as a stack), as well as the function 
definitions
- [ValueEnvironment.cs](ValueEnvironment.cs) - the class containing the Map of
variable names to values for a single environment
- [FunctionDefinition.cs](FunctionDefinition.cs) - the class containing a
function definition
- [Value.cs](Value.cs) - the class containing a value (an Integer)
- [Parser.cs](Parser.cs) - the entry-point into the parser and interpreter, providing the functions parseDefinition method, which returns the function definition name, and the parseExpression method, which returns the evaluated value of the expression.

The BasicEvaluator programming language consists of values and function 
definitions.  It only supports one type of value and that is an Integer.  Variables
are stored in the global environment.  Variable names can contain any character
except spaces, open and close parentheses, a semicolon, or tabs, carriage returns,
or line feeds.  Parentheses are used to delimit expressions and function 
expressions, much like it does in the family of programming languages based on LISP.
Semicolons are used to start a comment which goes to the end of the current line.
When a function is called, the actual arguments passed to the function are 
evaluated to a value, and then the formal arguments of the function are set to 
these actual argument values inside a local environment for that function.  The 
**visitFunctionExpr** method in the [BasicEvaluatorVisitorImpl.java](BasicEvaluatorVisitorImpl.java) 
file shows this logic, which is the only complicated part of the Interpreter.
All other visitor functions used to implement the Interpreter are pretty 
straightforward.  The use of a stack for the local environments fully support 
recursive function calls.  The last function definition in the [test01.lp](test01.lp) 
file is a recursive function definition for the Greatest Common Divisor 
algorithm (gcd).

### TODO
1. Better handling of parser errors
2. Add a **read** function to the language that reads input from the prompt
3. Add a **for** loop operation to the language
4. Experiment with passing values by reference during function calls, instead
   of by value, as it currently does.
5. Add a **load** function to the REPL that loads a file to be evaluated
6. Add local variables to functions
7. Add real numbers to the language
8. Implement static type-checking to the language as well as type-checking to
   distinguish between *statements* and *expressions* and between *procedures* 
   and *functions*
9. Updates to the REPL: a) history of commands and use of up/down arrow keys to 
   go through history, b) auto-indent, c) better editing capabilities for multi-
   line command entry to edit lines above or below current line in command.