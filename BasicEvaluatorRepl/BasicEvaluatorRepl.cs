using BasicEvaluatorInterpreter;

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
            string strInput = GetInput();

            if (strInput.Trim().Equals("quit"))
                quittingTime = true;
            else if (strInput.Trim().Equals("clear"))
                memory.Clear();
            else if (strInput.Length >= 7 && strInput.Trim().Substring(1, 6).Equals("define"))
            {
                string functionName = Parser.ParseDefinition(strInput, memory);
                if (!string.IsNullOrWhiteSpace(functionName))
                    Console.Out.WriteLine(functionName);
            }
            else
            {
                Value value = Parser.ParseExpression(strInput, memory);
                if (value.IsDefined)
                    Console.Out.WriteLine(value.GetValue);
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
}