using System.IO;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AbpSetup.Services;
using System.Collections.Generic;
using Spectre.Console;

public class CreateCommand : ConsoleAppBase
{
    readonly ILogger<CreateCommand> logger;
    private readonly ScriptService scriptService;

    public CreateCommand(ILogger<CreateCommand> logger, ScriptService scriptService)
    {
        this.logger = logger;
        this.scriptService = scriptService;
    }

    [Command("create", "Create a new microservice project.")]
    public async Task RunAsync([Option(0)] string input, [Option("o", "Output folder")] string output)
    {
        var scriptLocation = @".\Scripts\init.ps1";
        if (!File.Exists(scriptLocation))
        {
            logger.LogError($"{scriptLocation} not found.");
            return;
        }
        Console.WriteLine($"Script found: {scriptLocation}");
        await AnsiConsole.Status()
        .StartAsync("Creating...", async ctx =>
        {
            await scriptService.ExecuteAsync(scriptLocation, new Dictionary<string, object>
            {
                { "name", input },
                { "output", output }
            });
        });
    }
}