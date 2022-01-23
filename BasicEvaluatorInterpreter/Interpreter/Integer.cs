namespace BasicEvaluatorInterpreter.Interpreter;

public class Integer : Value
{
    public Integer(int value) : base(value)
    {
        // nothing to do
    }

    public override Value True => new Integer(1);
    public override Value False => new Integer(0);

    public override bool IsTrue()
    {
        return (int) DataValue != 0;
    }

    public override bool IsFalse()
    {
        return (int) DataValue == 0;
    }

    public override Value Add(Value operand)
    {
        return new Integer((int) DataValue + (int) operand.DataValue);
    }

    public override Value Sub(Value operand)
    {
        return new Integer((int) DataValue - (int) operand.DataValue);
    }

    public override Value Mul(Value operand)
    {
        return new Integer((int) DataValue * (int) operand.DataValue);
    }

    public override Value Div(Value operand)
    {
        return new Integer((int) DataValue / (int) operand.DataValue);
    }

    public override Value Eq(Value operand)
    {
        return (int) DataValue == (int) operand.DataValue ? True : False;
    }

    public override Value Lt(Value operand)
    {
        return (int) DataValue < (int) operand.DataValue ? True : False;
    }

    public override Value Gt(Value operand)
    {
        return (int) DataValue > (int) operand.DataValue ? True : False;
    }

    // public override string ToString()
    // {
    //     return ((int)DataValue).ToString();
    // }
}