namespace BasicEvaluatorInterpreter.Interpreter;

public class Memory
{
    private readonly ValueEnvironment _globals = new ValueEnvironment();
    private readonly Stack<ValueEnvironment> _functionStack = new Stack<ValueEnvironment>();
    private readonly Dictionary<string, FunctionDefinition> _functions = new Dictionary<string, FunctionDefinition>();

    public void AddLocalEnvironment()
    {
        _functionStack.Push(new ValueEnvironment());
    }

    public void RemoveLocalEnvironment()
    {
        _functionStack.Pop();
    }

    public void Clear()
    {
        _functionStack.Clear();
        _globals.ClearValues();
        _functions.Clear();
    }

    public Value? GetSymbol(string name)
    {
        // Step #1: check top of function stack for symbol
        if (_functionStack.Count > 0)
        {
            ValueEnvironment currentFunctionStack = _functionStack.Peek();
            Value? value = currentFunctionStack.GetValue(name);
            if (value != null) return value;
        }
        
        // STep #2: check globals for symbol
        return _globals.GetValue(name);
    }

    public void SetSymbol(String name, Value value)
    {
        if (_functionStack.Count > 0)
        {
            ValueEnvironment functionEnvironment = _functionStack.Peek();
            if (functionEnvironment.IsBound(name))
            {
                functionEnvironment.PutValue(name, value);
                return;
            }
        }
        
        _globals.PutValue(name, value);
    }

    public void SetLocalSymbol(String name, Value value)
    {
        if (_functionStack.Count > 0)
        {
            ValueEnvironment functionEnvironment = _functionStack.Peek();
            functionEnvironment.PutValue(name, value);
        }
    }

    public FunctionDefinition? GetFunction(String functionName)
    {
        return _functions.ContainsKey(functionName) 
            ? _functions[functionName] 
            : null;
    }

    public void SetFunction(String functionName, FunctionDefinition functionDef)
    {
        if (_functions.ContainsKey(functionName))
            _functions[functionName] = functionDef;
        else
            _functions.Add(functionName, functionDef);
    }
}