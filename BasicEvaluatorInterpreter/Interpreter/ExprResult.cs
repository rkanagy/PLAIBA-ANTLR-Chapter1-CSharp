namespace BasicEvaluatorInterpreter.Interpreter;

public class ExprResult : Expression
{
    public ExprResult(Value result)
    {
        Result = result;
    }
    
    public Value Result { get; }
}