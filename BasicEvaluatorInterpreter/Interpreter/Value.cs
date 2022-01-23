namespace BasicEvaluatorInterpreter.Interpreter;

public abstract class Value
{
    protected Value(object value)
    {
        DataValue = value;
    }
    
    public object DataValue { get; }

    public abstract Value True { get; }
    public abstract Value False { get; }

    public abstract bool IsTrue();
    public abstract bool IsFalse();

    public abstract Value Add(Value operand);
    public abstract Value Sub(Value operand);
    public abstract Value Mul(Value operand);
    public abstract Value Div(Value operand);
    public abstract Value Eq(Value operand);
    public abstract Value Lt(Value operand);
    public abstract Value Gt(Value operand);

    //public new abstract string ToString();

    public Value Print()
    {
        string? strValue = DataValue.ToString();
        Console.Out.WriteLine(strValue);
        return this;
    }
}