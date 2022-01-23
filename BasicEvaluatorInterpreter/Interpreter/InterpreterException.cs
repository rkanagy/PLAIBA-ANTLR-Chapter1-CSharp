namespace BasicEvaluatorInterpreter.Interpreter;

public class InterpreterException : Exception
{
    public InterpreterException()
    {
    }

    public InterpreterException(string message) : base(message)
    {
    }

    public InterpreterException(string message, Exception inner) : base(message, inner)
    {
    }
}