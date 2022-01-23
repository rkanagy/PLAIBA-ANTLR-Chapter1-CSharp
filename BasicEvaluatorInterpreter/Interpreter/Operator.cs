namespace BasicEvaluatorInterpreter.Interpreter;

public class Operator : Operation
{
    public Operator(string operatorType)
    {
        OperatorType = operatorType;
    }
    
    public string OperatorType { get; }
}