using CliWrap;

namespace AbpSetup.Services;

public class CliService
{
    public async Task RunAsync(string command, string args)
    {
        await using var stdOut = Console.OpenStandardOutput();
        await using var stdErr = Console.OpenStandardError();
        var cmd = Cli.Wrap(command).WithArguments(args).WithWorkingDirectory(Directory.GetCurrentDirectory()) | (stdOut, stdErr);
        await cmd.ExecuteAsync();
    }
}