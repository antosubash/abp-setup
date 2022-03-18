using System;
using Microsoft.Extensions.Logging;

public class TestCommand : ConsoleAppBase
{
    private ILogger<TestCommand> logger;

    public TestCommand(ILogger<TestCommand> logger)
    {
        this.logger = logger;
    }

    [Command("test", "Test command.")]
    public void Run([Option(0)] string input)
    {
        // Context has any useful information.
        Console.WriteLine($"Test input: {input}");
    }
}