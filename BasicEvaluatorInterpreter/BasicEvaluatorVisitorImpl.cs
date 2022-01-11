namespace BasicEvaluatorInterpreter;

public class BasicEvaluatorVisitorImpl : BasicEvaluatorBaseVisitor<BasicEvaluatorInfo>
{
    private readonly Memory _memory;
    private readonly HashSet<string> _keywords = new HashSet<string>();

    public BasicEvaluatorVisitorImpl(Memory memory)
    {
        this._memory = memory;
        InstallKeywords();
    }

    private void InstallKeywords()
    {
        _keywords.Add("define");
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

    public override BasicEvaluatorInfo VisitFunDef(BasicEvaluatorParser.FunDefContext context)
    {
        // get function name
        string functionName = context.function().GetText();
        if (_keywords.Contains(functionName))
        {
            Console.Out.WriteLine("Invalid function name: " + functionName);
            return new BasicEvaluatorInfo("");
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

        return new BasicEvaluatorInfo(functionName);
    }

    public override BasicEvaluatorInfo VisitValueExpr(BasicEvaluatorParser.ValueExprContext context)
    {
        return Visit(context.value());
    }

    public override BasicEvaluatorInfo VisitVariableExpr(BasicEvaluatorParser.VariableExprContext context)
    {
        string variableName = context.variable().GetText();
        Value? value = _memory.GetSymbol(variableName);
        if (value != null) return new BasicEvaluatorInfo(value);
        
        Console.Out.WriteLine("Undefined variable: " + variableName);
        return new BasicEvaluatorInfo(new Value(null));
    }

    public override BasicEvaluatorInfo VisitIfExpr(BasicEvaluatorParser.IfExprContext context)
    {
        BasicEvaluatorInfo comparison = Visit(context.expression(0));
        return Visit(comparison.Value.IsFalse 
            ? context.expression(2) // else expression
            : context.expression(1)); // then expression
    }

    public override BasicEvaluatorInfo VisitWhileExpr(BasicEvaluatorParser.WhileExprContext context)
    {
        BasicEvaluatorInfo condition = Visit(context.expression(0));
        while (condition.Value.IsTrue)
        {
            Visit(context.expression(1));
            condition = Visit(context.expression(0));
        }
        return condition;
    }

    public override BasicEvaluatorInfo VisitSetExpr(BasicEvaluatorParser.SetExprContext context)
    {
        string variable = context.variable().GetText();
        BasicEvaluatorInfo value = Visit(context.expression());
        _memory.SetSymbol(variable, value.Value);
        return value;
    }

    public override BasicEvaluatorInfo VisitBeginExpr(BasicEvaluatorParser.BeginExprContext context)
    {
        BasicEvaluatorInfo currentValue = Visit(context.expression(0));
        
        int i = 1;
        while (true)
        {
            if (context.expression(i) != null)
                currentValue = Visit(context.expression(i++));
            else
                break;
        }

        return currentValue;
    }

    public override BasicEvaluatorInfo VisitFunctionExpr(BasicEvaluatorParser.FunctionExprContext context)
    {
        string functionName = context.function().GetText();
        FunctionDefinition? function = _memory.GetFunction(functionName);
        if (function != null)
        {
            // Step #1: check actual arg count with formal arg count
            int argCount = function.ArgumentList.Count;
            if (argCount != context.expression().Length)
            {
                Console.Out.WriteLine("Wrong number of arguments to: " + functionName);
                return new BasicEvaluatorInfo(new Value(null));
            }
            
            // Step #2: get values of actual arguments
            Value[] actualArgs = new Value[argCount];
            int i = 0;
            while (context.expression(i) != null)
            {
                BasicEvaluatorInfo argValue = Visit(context.expression(i));
                actualArgs[i] = argValue.Value;
                i++;
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
            BasicEvaluatorInfo functionValue = Visit(function.Expression);
            
            // Step #6: remove local environment
            _memory.RemoveLocalEnvironment();

            return functionValue;
        }

        Console.Out.WriteLine("Undefined function: " + functionName);
        return new BasicEvaluatorInfo(new Value(null));
    }

    public override BasicEvaluatorInfo VisitOperatorExpr(BasicEvaluatorParser.OperatorExprContext context)
    {
        BasicEvaluatorInfo left = Visit(context.expression(0));
        BasicEvaluatorInfo right = Visit(context.expression(1));
        Value leftValue = left.Value;
        Value rightValue = right.Value;

        return context.op.Type switch
        {
            BasicEvaluatorParser.ADD => new BasicEvaluatorInfo(new Value(leftValue.GetValue + rightValue.GetValue)),
            BasicEvaluatorParser.SUB => new BasicEvaluatorInfo(new Value(leftValue.GetValue - rightValue.GetValue)),
            BasicEvaluatorParser.MUL => new BasicEvaluatorInfo(new Value(leftValue.GetValue * rightValue.GetValue)),
            BasicEvaluatorParser.DIV => new BasicEvaluatorInfo(new Value(leftValue.GetValue / rightValue.GetValue)),
            BasicEvaluatorParser.EQ => new BasicEvaluatorInfo(leftValue.GetValue == rightValue.GetValue
                ? Value.True
                : Value.False),
            BasicEvaluatorParser.LT => new BasicEvaluatorInfo(leftValue.GetValue < rightValue.GetValue
                ? Value.True
                : Value.False),
            BasicEvaluatorParser.GT => new BasicEvaluatorInfo(leftValue.GetValue > rightValue.GetValue
                ? Value.True
                : Value.False),
            _ => new BasicEvaluatorInfo(new Value(null))
        };
    }

    public override BasicEvaluatorInfo VisitPrintExpr(BasicEvaluatorParser.PrintExprContext context)
    {
        BasicEvaluatorInfo value = Visit(context.expression());
        Console.Out.WriteLine(value.Value.GetValue);
        return value;
    }

    public override BasicEvaluatorInfo VisitValue(BasicEvaluatorParser.ValueContext context)
    {
        return new BasicEvaluatorInfo(new Value(Int32.Parse(context.GetText())));
    }
}