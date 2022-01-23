using BasicEvaluatorInterpreter.Parser;

namespace BasicEvaluatorInterpreter.Interpreter;

public class BasicEvaluatorVisitorImpl : BasicEvaluatorBaseVisitor<EvaluatorInput>
{
    private readonly Memory _memory;
    private readonly HashSet<string> _keywords = new();

    public BasicEvaluatorVisitorImpl(Memory memory)
    {
        _memory = memory;
        InstallKeywords();
    }

    private void InstallKeywords()
    {
        _keywords.Add("define"); //=> DataValue == 0; // IsDefined && value == 0;
        _keywords.Add("if");
        _keywords.Add("while");
        _keywords.Add("set");
        _keywords.Add("begin");
        _keywords.Add("+");
        _keywords.Add("-");
        _keywords.Add("*");
        _keywords.Add("/");
        _keywords.Add("=");
        _keywords.Add("<");
        _keywords.Add(">");
        _keywords.Add("print");
    }

    public override FunctionDef VisitFunDef(BasicEvaluatorParser.FunDefContext context)
    {
        // get function name
        string functionName = context.function().GetText();
        if (_keywords.Contains(functionName))
        {
            throw new InterpreterException("Invalid function name: " + functionName);
        }

        // get function formal arguments
        List<string> argList = new List<string>();
        BasicEvaluatorParser.VariableContext[] arguments = context.argList().variable();
        foreach (BasicEvaluatorParser.VariableContext argument in arguments)
        {
            argList.Add(argument.GetText());
        }

        // get function expression
        BasicEvaluatorParser.ExpressionContext expression = context.expression();

        FunctionDefinition function = new FunctionDefinition(functionName, argList, expression);
        _memory.SetFunction(functionName, function);

        return new FunctionDef(functionName);
    }

    public override ExprResult VisitValueExpr(BasicEvaluatorParser.ValueExprContext context)
    {
        try
        {
            int value = Int32.Parse(context.value().GetText());
            return new ExprResult(new Integer(value));
        }
        catch (Exception err)
        {
            throw new InterpreterException("Value not an integer", err);
        }
    }

    public override ExprResult VisitVariableExpr(BasicEvaluatorParser.VariableExprContext context)
    {
        Variable variable = (Variable) Visit(context.variable());
        string variableName = variable.VariableName;
        Value? value = _memory.GetSymbol(variableName);
        if (value == null)
            throw new InterpreterException("Undefined variable: " + variableName);
        
        return new ExprResult(value);
    }

    public override ExprResult VisitIfExpr(BasicEvaluatorParser.IfExprContext context)
    {
        ExprResult comparison = (ExprResult)Visit(context.expression(0));
        if (comparison == null)
            throw new InterpreterException("if condition invalid");

        if (comparison.Result.IsTrue())
        {
            ExprResult result = (ExprResult) Visit(context.expression(1));
            if (result == null)
                throw new InterpreterException("if true clause invalid");
            return result;
        }
        else
        {
            ExprResult result = (ExprResult) Visit(context.expression(2));
            if (result == null)
                throw new InterpreterException("if false clause invalid");
            return result;
        }
    }

    public override ExprResult VisitWhileExpr(BasicEvaluatorParser.WhileExprContext context)
    {
        ExprResult condition = (ExprResult) Visit(context.expression(0));
        if (condition == null)
            throw new InterpreterException("while condition invalid");
        
        while (condition.Result.IsTrue())
        {
            ExprResult result = (ExprResult) Visit(context.expression(1));
            if (result == null)
                throw new InterpreterException("while loop invalid");
            
            condition = (ExprResult) Visit(context.expression(0));
        }
        return condition;
    }

    public override ExprResult VisitSetExpr(BasicEvaluatorParser.SetExprContext context)
    {
        Variable variable = (Variable) Visit(context.variable());
        if (variable == null)
            throw new InterpreterException("set variable invalid");

        ExprResult value = (ExprResult) Visit(context.expression());
        if (value == null)
            throw new InterpreterException("set expression invalid");
        
        _memory.SetSymbol(variable.VariableName, value.Result);
        
        return value;
    }

    public override ExprResult VisitBeginExpr(BasicEvaluatorParser.BeginExprContext context)
    {
        if (context.expression(0) == null)
            throw new InterpreterException("begin requires at least one expression");
        
        ExprResult currentValue = (ExprResult) Visit(context.expression(0));
        
        int i = 1;
        while (true)
        {
            if (context.expression(i) != null)
                currentValue = (ExprResult) Visit(context.expression(i++));
            else
                break;
        }

        return currentValue;
    }

    public override ExprResult VisitOperatorExpr(BasicEvaluatorParser.OperatorExprContext context)
    {
        Operation operation = (Operation) Visit(context.@operator());
        if (operation is Operator operatorType)
        {
            return ApplyOperator(operatorType.OperatorType, context.expression());
        }

        if (operation is FunctionCall functionCall)
        {
            return ApplyFunctionCall(functionCall.FunctionName, context.expression());
        }

        throw new InterpreterException("Invalid operator type");
    }
    
    public override Operation VisitFunctionExpr(BasicEvaluatorParser.FunctionExprContext context)
    {
        string functionName = context.function().GetText();
        return new FunctionCall(functionName);
        
    }

    public override Operation VisitValueOpExpr(BasicEvaluatorParser.ValueOpExprContext context)
    {
        string valueOp = context.valueOp().GetText();
        return new Operator(valueOp);
    }

    private ExprResult ApplyOperator(string operatorType, BasicEvaluatorParser.ExpressionContext[] expressions)
    {
        // check if correct number of operands were parsed
        int numOperands = expressions.Length;
        bool valid = (operatorType.Equals("print")) ? numOperands == 1 : numOperands == 2;
        if (!valid) throw new InterpreterException("Wrong number of arguments to " + operatorType);
        
        // apply operator
        Value op1Value = ((ExprResult) Visit(expressions[0])).Result;
        switch (operatorType)
        {
            // these operators have 2 operands
            case "+": case "-": case "*": case "/": case "=": case "<": case ">": 
                Value op2Value = ((ExprResult) Visit(expressions[1])).Result;
                switch (operatorType)
                {
                    case "+": return new ExprResult(op1Value.Add(op2Value)); 
                    case "-": return new ExprResult(op1Value.Sub(op2Value)); 
                    case "*": return new ExprResult(op1Value.Mul(op2Value)); 
                    case "/": return new ExprResult(op1Value.Div(op2Value)); 
                    case "=": return new ExprResult(op1Value.Eq(op2Value)); 
                    case "<": return new ExprResult(op1Value.Lt(op2Value)); 
                    case ">": return new ExprResult(op1Value.Gt(op2Value));
                }
                break;
            
            // these operators have 1 operand
            case "print":
                return new ExprResult(op1Value.Print());
        }

        // if it gets here, then we have an invalid operator
        throw new InterpreterException("Invalid operator: " + operatorType);
    }

    private ExprResult ApplyFunctionCall(string functionName, BasicEvaluatorParser.ExpressionContext[] expressions)
    {
        FunctionDefinition? function = _memory.GetFunction(functionName);
        if (function != null)
        {
            // Step #1: check actual arg count with formal arg count
            int argCount = function.ArgumentList.Count;
            if (argCount != expressions.Length)
            {
                throw new InterpreterException("Wrong number of arguments to: " + functionName);
            }
            
            // Step #2: get values of actual arguments
            Value[] actualArgs = new Value[argCount];
            for (int i = 0; i < argCount; i++)
            {
                ExprResult argValue = (ExprResult) Visit(expressions[i]);
                if (argValue == null)
                {
                    throw new InterpreterException("Invalid actual argument for formal argument " +
                                                   function.ArgumentList[i] +
                                                   " in function " +
                                                   functionName);
                }
                actualArgs[i] = argValue.Result;
            }
            
            // Step #3: create a new local environment
            _memory.AddLocalEnvironment();
            
            // Step #4: add actual args to formal args in local environment of function
            //          using the format argument names
            for (int j = 0; j < argCount; j++)
            {
                _memory.SetLocalSymbol(function.ArgumentList[j], actualArgs[j]);
            }
            
            // Step #5: execute function expression
            ExprResult functionValue = (ExprResult) Visit(function.Expression);
            
            // Step #6: remove local environment
            _memory.RemoveLocalEnvironment();

            return functionValue;
        }

        throw new InterpreterException("Undefined function: " + functionName);
    }
    
    public override EvaluatorInput VisitVariable(BasicEvaluatorParser.VariableContext context)
    {
        string variableName = context.GetText();
        if (context.exception != null) 
            throw new InterpreterException("Invalid variable: " + variableName);
        return new Variable(variableName);
    }
}