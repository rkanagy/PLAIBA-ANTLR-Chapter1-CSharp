namespace BasicEvaluatorInterpreter.Interpreter;

public class Variable : Expression
{
    public Variable(string variableName)
    {
        VariableName = variableName;
    }
    
    public string VariableName { get; }
}