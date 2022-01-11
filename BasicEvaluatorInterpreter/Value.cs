namespace BasicEvaluatorInterpreter;

public class Value
{
    private int? value;

    public Value(int? value)
    {
        this.value = value;
    }
    
    public static Value True => new Value(1);
    public static Value False => new Value(0);

    public int? GetValue => value;
    public bool IsDefined => value != null;
    public bool IsTrue => IsDefined && value != 0 ;
    public bool IsFalse => IsDefined && value == 0;
}