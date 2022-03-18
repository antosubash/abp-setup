using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AbpSetup.Services;

public class CreateCommand : ConsoleAppBase
{
    readonly ILogger<CreateCommand> logger;
    private readonly HelloWorldService helloWorldService;

    //  You can receive DI services in constructor.
    public CreateCommand(ILogger<CreateCommand> logger, HelloWorldService helloWorldService)
    {
        this.logger = logger;
        this.helloWorldService = helloWorldService;
    }

    [Command("create", "Create a new microservice project.")]
    public async Task RunAsync([Option(0)] string input) 
    {
        // Context has any useful information.
        Console.WriteLine($"Input: {input}");
        await helloWorldService.SayHelloAsync();;
    }

}