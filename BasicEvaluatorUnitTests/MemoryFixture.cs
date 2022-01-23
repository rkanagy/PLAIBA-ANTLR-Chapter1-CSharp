using System;
using BasicEvaluatorInterpreter.Interpreter;

namespace BasicEvaluatorUnitTests;

public class MemoryFixture : IDisposable
{
    public MemoryFixture()
    {
        SharedMemory = new Memory();
    }

    public void Dispose()
    {
        SharedMemory.Clear();
    }
    
    public Memory SharedMemory { get; private set; }
}
