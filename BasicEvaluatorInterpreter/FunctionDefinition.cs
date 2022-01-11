namespace BasicEvaluatorInterpreter;

public class FunctionDefinition
{
    public FunctionDefinition(string functionName,  List<string> argumentList, 
        BasicEvaluatorParser.ExpressionContext expression)
    {
        FunctionName = functionName;
        ArgumentList = argumentList;
        Expression = expression;
    }
    
    public string FunctionName { get; }

    public List<string> ArgumentList { get; }

    public BasicEvaluatorParser.ExpressionContext Expression { get; }
}