using Antlr4.Runtime;

namespace BasicEvaluatorInterpreter;

public class Parser
{
    public static string ParseDefinition(string strInput, Memory memory)
    {
        BasicEvaluatorParser parser = GetParser(strInput);
        BasicEvaluatorParser.FunDefContext tree = parser.funDef();

        BasicEvaluatorVisitorImpl visitor = new BasicEvaluatorVisitorImpl(memory);
        BasicEvaluatorInfo info = visitor.Visit(tree);

        return info.FunctionName;
    }

    public static Value ParseExpression(string strInput, Memory memory)
    {
        BasicEvaluatorParser parser = GetParser(strInput);
        BasicEvaluatorParser.ExpressionContext tree = parser.expression();

        BasicEvaluatorVisitorImpl visitor = new BasicEvaluatorVisitorImpl(memory);
        BasicEvaluatorInfo info = visitor.Visit(tree);

        return info.Value;
    }

    private static BasicEvaluatorParser GetParser(string strInput)
    {
        ICharStream input = CharStreams.fromString(strInput);
        BasicEvaluatorLexer lexer = new BasicEvaluatorLexer(input);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        return new BasicEvaluatorParser(tokens);
    }
}