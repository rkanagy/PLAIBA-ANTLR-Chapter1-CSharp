namespace BasicEvaluatorInterpreter;

public class ValueEnvironment
{
    private readonly Dictionary<string, Value?> _environment = new Dictionary<string, Value?>();

    public Value? GetValue(string name)
    {
        return _environment.ContainsKey(name) ? _environment[name] : null;
    }

    public void PutValue(string name, Value value)
    {
        if (_environment.ContainsKey(name))
            _environment[name] = value;
        else
            _environment.Add(name, value);
    }

    public bool IsBound(string name)
    {
        return _environment.ContainsKey(name);
    }

    public void ClearValues()
    {
        _environment.Clear();
    }
}