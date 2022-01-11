namespace BasicEvaluatorInterpreter;

public class BasicEvaluatorInfo
{
    public BasicEvaluatorInfo(string functionName)
    {
        FunctionName = functionName;
        Value = new Value(null);
    }

    public BasicEvaluatorInfo(Value value)
    {
        FunctionName = "";
        Value = value;
    }

    public string FunctionName { get; }

    public Value Value { get; }
}