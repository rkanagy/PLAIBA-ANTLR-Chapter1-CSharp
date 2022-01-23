using Antlr4.Runtime;
using BasicEvaluatorInterpreter.Interpreter;
using BasicEvaluatorInterpreter.Parser;

namespace BasicEvaluatorInterpreter;

public class LanguageParser
{
    public static EvaluatorInput ParseInput(string strInput, Memory memory)
    {
        ICharStream input = CharStreams.fromString(strInput);
        BasicEvaluatorLexer lexer = new BasicEvaluatorLexer(input);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        BasicEvaluatorParser parser = new BasicEvaluatorParser(tokens);
        BasicEvaluatorParser.ProgContext tree = parser.prog();

        BasicEvaluatorVisitorImpl visitor = new BasicEvaluatorVisitorImpl(memory);
        return visitor.Visit(tree);
    }
}