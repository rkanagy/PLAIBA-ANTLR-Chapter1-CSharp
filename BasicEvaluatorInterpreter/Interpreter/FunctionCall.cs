namespace BasicEvaluatorInterpreter.Interpreter;

public class FunctionCall : Operation
{
    public FunctionCall(string functionName)
    {
        FunctionName = functionName;
    }
    
    public string FunctionName { get; }
}