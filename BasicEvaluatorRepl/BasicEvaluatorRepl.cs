using BasicEvaluatorInterpreter;
using BasicEvaluatorInterpreter.Interpreter;

namespace BasicEvaluatorRepl;

public static class BasicEvaluatorRepl
{
    public static void ReadEvalPrintLoop()
    {
        Memory memory = new Memory();
        
        // Read-Eval-Print loop
        bool quittingTime = false;
        while (!quittingTime)
        {
            // Read
            string strInput = GetInput();
            
            switch (strInput.Trim())
            {
                case "quit":
                    quittingTime = true;
                    break;
                case "clear":
                    memory.Clear();
                    break;
                default:
                    // Eval
                    EvaluatorInput? result = EvaluateInput(strInput, memory);
                    
                    // Print
                    if (result != null) PrintResult(result);
                    break;
            }
        }
    }

    private static string GetInput()
    {
        int parenCount = 0;
        string prompt = "-> ";
        List<string> lines = new List<string>();

        do
        {
            Console.Out.Write(prompt);
            string? line = Console.In.ReadLine();
            if (line != null)
            {
                line = line.TrimEnd();

                foreach (var chr in line)
                {
                    if (chr == '(')
                        parenCount++;
                    else if (chr == ')')
                        parenCount--;
                }
                
                lines.Add(line);
                prompt = "> ";
            }
        } while (parenCount != 0);

        return string.Join("\n", lines);
    }

    private static EvaluatorInput? EvaluateInput(string strInput, Memory memory)
    {
        try
        {
            return LanguageParser.ParseInput(strInput, memory);
        }
        catch (InterpreterException err)
        {
            Console.Out.WriteLine(err.Message);
        }

        return null;
    }
    
    private static void PrintResult(EvaluatorInput result)
    {
        if (result is FunctionDef functionDef)
        {
            string functionName = functionDef.Name;
            if (!string.IsNullOrWhiteSpace(functionName))
            {
                Console.Out.WriteLine(functionName);
            }
        }
        else if (result is ExprResult exprResult)
        {
            Value value = exprResult.Result;
            Console.Out.WriteLine(value.DataValue);
        }

        Console.Out.WriteLine();
    }
}