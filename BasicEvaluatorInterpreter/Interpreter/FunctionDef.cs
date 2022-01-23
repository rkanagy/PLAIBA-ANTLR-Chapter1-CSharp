namespace BasicEvaluatorInterpreter.Interpreter;

public class FunctionDef : EvaluatorInput
{
    public FunctionDef(string name)
    {
        Name = name;
    }
    
    public string Name { get; }
}