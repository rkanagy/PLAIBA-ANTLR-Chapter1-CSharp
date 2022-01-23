using Xunit;
using BasicEvaluatorInterpreter;
using BasicEvaluatorInterpreter.Interpreter;
using BasicEvaluatorUnitTests.TestCaseOrderer;

namespace BasicEvaluatorUnitTests;

[TestCaseOrderer("BasicEvaluatorUnitTests.TestCaseOrderer.PriorityOrderer", 
    "BasicEvaluatorUnitTests")]
public class InterpreterTests : IClassFixture<MemoryFixture>
{
    private MemoryFixture sharedMemory;
    public static bool test1Run = false;
    public static bool test2Run = false;
    public static bool test3Run = false;
    public static bool test4Run = false;
    
    public InterpreterTests(MemoryFixture memory)
    {
        sharedMemory = memory;
    }
    
    [Fact, TestPriority(1)]
    public void InterpretIntegerValueTest()
    {
        string strInput = "3";
        TestIntegerExpression(strInput, 3);

        test1Run = true;
        Assert.False(test2Run);
        Assert.False(test3Run);
        Assert.False(test4Run);
    }
    
    [Fact, TestPriority(2)]
    public void InterpretIntegerAddTest()
    {
        string input = "(+ 4 7)";
        TestIntegerExpression(input, 11);

        test2Run = true;
        Assert.True(test1Run);
        Assert.False(test3Run);
        Assert.False(test4Run);
    }
    
    [Fact, TestPriority(3)]
    public void InterpreterSetVariableTest()
    {
        string input = "(set x 4)";
        TestIntegerExpression(input, 4);

        test3Run = true;
        Assert.True(test1Run);
        Assert.True(test2Run);
        Assert.False(test4Run);
    }
    
    [Fact, TestPriority(4)]
    public void InterpretAddIntegerVariablesTest()
    {
        string input = "(+ x x)";
        TestIntegerExpression(input, 8);

        test4Run = true;
        Assert.True(test1Run);
        Assert.True(test2Run);
        Assert.True(test3Run);
    }

    [Fact, TestPriority(5)]
    public void InterpreterPrintVariableTest()
    {
        string input = "(print x)";
        TestIntegerExpression(input, 4);
    }

    [Fact, TestPriority(6)]
    public void InterpreterSetVariableTest2()
    {
        string input = "(set y 5)";
        TestIntegerExpression(input, 5);
    }

    [Fact, TestPriority(7)]
    public void InterpreterBeginTest()
    {
        string input = "(begin (print x) (print y) (* x y))";
        TestIntegerExpression(input, 20);
    }

    [Fact, TestPriority(8)]
    public void InterpreterIfTest()
    {
        string input = "(if (> y 0) 5 10)";
        TestIntegerExpression(input, 5);
    }

    [Fact, TestPriority(9)]
    public void InterpreterWhileTest()
    {
        string input = "(while (> y 0) (begin (set x (+ x x)) (set y (- y 1))))";
        TestIntegerExpression(input, 0);
    }

    [Fact, TestPriority(10)]
    public void InterpreterVariableValueTest()
    {
        string input = "x";
        TestIntegerExpression(input, 128);
    }

    [Fact, TestPriority(11)]
    public void InterpreterDefineFunctionTest()
    {
        string input = "(define +1 (x) (+ x 1))";
        TestFunctionDefinition(input, "+1");
    }

    [Fact, TestPriority(12)]
    public void InterpreterCallFunctionTest()
    {
        string strInput = "(+1 4)";
        TestIntegerExpression(strInput, 5);
    }

    [Fact, TestPriority(13)]
    public void InterpreterDefineFunctionTest2()
    {
        string strInput = "(define double (x) (+ x x))";
        TestFunctionDefinition(strInput, "double");
    }

    [Fact, TestPriority(14)]
    public void InterpreterCallFunctionTest2()
    {
        string strInput = "(double 4)";
        TestIntegerExpression(strInput, 8);
    }

    [Fact, TestPriority(15)]
    public void InterpreterVariableValueTest2()
    {
        string input = "x";
        TestIntegerExpression(input, 128);
    }

    [Fact, TestPriority(16)]
    public void InterpreterDefineFunctionTest3()
    {
        string input = "(define setx (x y) (begin (set x (+ x y)) x))";
        TestFunctionDefinition(input, "setx");
    }

    [Fact, TestPriority(17)]
    public void InterpreterCallFunctionTest3()
    {
        string input = "(setx x 1)";
        TestIntegerExpression(input, 129);
    }

    [Fact, TestPriority(18)]
    public void InterpreterVariableValueTest3()
    {
        string input = "x";
        TestIntegerExpression(input, 128);
    }

    [Fact, TestPriority(19)]
    public void InterpreterDefineFunctionTest4()
    {
        string input = "(define not (boolval) (if boolval 0 1))";
        TestFunctionDefinition(input, "not");
    }

    [Fact, TestPriority(20)]
    public void InterpreterDefineFunctionTest5()
    {
        string input = "(define <> (x y) (not (= x y)))";
        TestFunctionDefinition(input, "<>");
    }

    [Fact, TestPriority(21)]
    public void InterpreterDefineFunctionTest6()
    {
        string input = "(define mod (m n) (- m (* n (/ m n))))";
        TestFunctionDefinition(input, "mod");
    }

    [Fact, TestPriority(22)]
    public void InterpreterDefineFunctionTest7()
    {
        string input = @"(define gcd (m n)
                             (begin
                                (set r (mod m n))
                                (while (<> r 0)
                                   (begin
                                      (set m n)
                                      (set n r)
                                      (set r (mod m n))))
                                n))";
        TestFunctionDefinition(input, "gcd");
    }

    [Fact, TestPriority(23)]
    public void InterpreterCallFunctionTest4()
    {
        string input = "(gcd 6 15)";
        TestIntegerExpression(input, 3);
    }

    [Fact, TestPriority(24)]
    public void InterpreterDefineFunctionTest8()
    {
        string input = @"(define gcd2 (m n)
                            (if (= n 0) m (gcd2 n (mod m n))))";
        TestFunctionDefinition(input, "gcd2");
    }

    [Fact, TestPriority(25)]
    public void InterpreterCallFunctionTest5()
    {
        string input = "(gcd2 6 15)";
        TestIntegerExpression(input, 3);
    }
    
    private void TestIntegerExpression(string strInput, int result)
    {
        Memory memory = sharedMemory.SharedMemory;
        EvaluatorInput exprResult = LanguageParser.ParseInput(strInput, memory);
        
        Assert.True(exprResult is ExprResult);
        Value value = ((ExprResult) exprResult).Result;
        Assert.True(value is Integer);
        Assert.Equal(result, value.DataValue);
    }

    private void TestFunctionDefinition(string strInput, string functionName)
    {
        Memory memory = sharedMemory.SharedMemory;
        EvaluatorInput result = LanguageParser.ParseInput(strInput, memory);
        
        Assert.True(result is FunctionDef);
        string funDefName = ((FunctionDef) result).Name;
        Assert.Equal(functionName, funDefName);
    }
}