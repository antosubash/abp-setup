using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using System.Management.Automation;
using PowerShell = System.Management.Automation.PowerShell;
using System.Threading.Tasks;
using Kurukuru;

public class ScriptService : ITransientDependency
{
    private readonly ILogger<ScriptService> logger;

    public ScriptService(ILogger<ScriptService> logger)
    {
        this.logger = logger;
    }

    public async Task<PSDataCollection<PSObject>> ExecuteAsync(string scriptPath, IDictionary parameters)
    {
        if (!File.Exists(scriptPath))
            throw new ArgumentException("The script specified does not exist");

        string rawScript = File.ReadAllText(scriptPath);
        using PowerShell posh = PowerShell.Create().AddScript(rawScript);
        foreach (DictionaryEntry de in parameters)
        {
            posh.AddParameter((string)de.Key, de.Value);
        }
        posh.Streams.Information.DataAdded += (sender, e) =>
        {
            var info = posh.Streams.Information[e.Index];
            logger.LogInformation(info.ToString());
        };
        posh.Streams.Error.DataAdded += (sender, e) =>
        {
            var error = posh.Streams.Error[e.Index];
            logger.LogError(error.ToString());
        };
        posh.Streams.Warning.DataAdded += (sender, e) =>
        {
            var warning = posh.Streams.Warning[e.Index];
            logger.LogWarning(warning.ToString());
        };
        posh.Streams.Verbose.DataAdded += (sender, e) =>
        {
            var verbose = posh.Streams.Verbose[e.Index];
            logger.LogDebug(verbose.ToString());
        };
        posh.Streams.Debug.DataAdded += (sender, e) =>
        {
            var debug = posh.Streams.Debug[e.Index];
            logger.LogDebug(debug.ToString());
        };
        return await posh.InvokeAsync();
    }
}