using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using System.Management.Automation;
using PowerShell = System.Management.Automation.PowerShell;
public class ScriptService : ITransientDependency
{
    private readonly ILogger<ScriptService> logger;

    public ScriptService(ILogger<ScriptService> logger)
    {
        this.logger = logger;
    }

    public static Collection<PSObject> Execute(string scriptPath, IDictionary parameters)
    {
        if (!File.Exists(scriptPath))
            throw new ArgumentException("The script specified does not exist");

        string rawScript = File.ReadAllText(scriptPath);
        using (PowerShell posh = PowerShell.Create().AddScript(rawScript))
        {
            foreach (DictionaryEntry de in parameters)
            {
                posh.AddParameter((string)de.Key, de.Value);
            }
            return posh.Invoke();
        }
    }
}