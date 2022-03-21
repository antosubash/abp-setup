using System.IO;
using System.ComponentModel;
using AbpSetup.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AbpSetup.Commands
{
    public class CreateCommand : AsyncCommand<CreateCommand.Settings>
    {
        private readonly CliService cliService;

        public CreateCommand(CliService cliService)
        {
            this.cliService = cliService;
        }
        public sealed class Settings : CommandSettings
        {
            [Description("The name of the project")]
            [CommandArgument(0, "[Name]")]
            public string? Name { get; set; }

            [Description("The location of the project")]
            [CommandOption("-o|--output <OUTPUT>")]
            public string? Output { get; set; }
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            if (settings.Name is null)
            {
                AnsiConsole.WriteException(new ArgumentNullException("Name is required"));
                return 1;
            }

            if (settings.Output is null)
            {
                AnsiConsole.WriteLine("Creating project in current directory");
            }
            else
            {
                AnsiConsole.WriteLine($"Creating project in {Path.Combine(Directory.GetCurrentDirectory(), settings.Output)}");
            }

            string dir = @"C:\test";

            try
            {
                //Set the current directory.
                Directory.SetCurrentDirectory(dir);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("The specified directory does not exist. {0}", e);
            }
            //await cliService.RunAsync("powershell.exe", "mkdir apps");
            await cliService.RunAsync("dotnet", $"new web -n {settings.Name}.IdentityServer -o apps\\{settings.Name}.IdentityServer");
            await cliService.RunAsync("dotnet", $"new web -n {settings.Name}.Gateway -o gateway\\{settings.Name}.Gateway");
            await cliService.RunAsync("dotnet", $"new classlib -n {settings.Name}.Shared.Hosting -o shared\\{settings.Name}.Shared.Hosting");
            await cliService.RunAsync("dotnet", $"new console -n {settings.Name}.DbMigrator -o shared\\{settings.Name}.DbMigrator");
            await cliService.RunAsync("abp", $"new {settings.Name}.AdministrationService -t module --no-ui -o services\\administration");
            await cliService.RunAsync("abp", $"new {settings.Name}.IdentityService -t module --no-ui -o services\\identity");
            await cliService.RunAsync("abp", $"new {settings.Name}.SaaSService -t module --no-ui -o services\\saas");
            await cliService.RunAsync("dotnet", $"new sln -n {settings.Name}");
            await cliService.RunAsync("powershell.exe", $"dotnet sln .\\{settings.Name}.sln add (Get-ChildItem -r **/*.csproj)");
            await cliService.RunAsync("abp", $"new {settings.Name} -t app -u angular -dbms PostgreSQL -m none --separate-identity-server --database-provider ef -csf -o temp");
            await cliService.RunAsync("powershell.exe", $"Move-Item -Path .\\temp\\{settings.Name}\\angular\\ -Destination .\\apps\\angular -Force");
            await cliService.RunAsync("powershell.exe", $"Remove-Item -Recurse -Force .\\shared\\{settings.Name}.DbMigrator");
            await cliService.RunAsync("powershell.exe", $"Remove-Item -Recurse -Force .\\apps\\{settings.Name}.IdentityServer");
            await cliService.RunAsync("powershell.exe", $"Move-Item -Path .\\temp\\{settings.Name}\\aspnet-core\\src\\{settings.Name}.DbMigrator -Destination .\\shared -Force");
            await cliService.RunAsync("powershell.exe", $"Move-Item -Path .\\temp\\{settings.Name}\\aspnet-core\\src\\{settings.Name}.IdentityServer -Destination .\\apps -Force");
            await cliService.RunAsync("powershell.exe", $"Remove-Item -Recurse -Force .\\temp\\");
            await cliService.RunAsync("powershell.exe", $"dotnet sln .\\{settings.Name}.sln remove (Get-ChildItem -r **/*.Installer.csproj)");
            await cliService.RunAsync("powershell.exe", $"dotnet sln .\\{settings.Name}.sln remove (Get-ChildItem -r **/*.Host.Shared.csproj)");
            await cliService.RunAsync("powershell.exe", $"dotnet sln .\\{settings.Name}.sln remove (Get-ChildItem -r **/*.MongoDB.csproj)");
            await cliService.RunAsync("powershell.exe", $"dotnet sln .\\{settings.Name}.sln remove (Get-ChildItem -r **/*.MongoDB.Tests.csproj)");
            await cliService.RunAsync("powershell.exe", $"dotnet sln .\\{settings.Name}.sln remove (Get-ChildItem -r **/*.AdministrationService.IdentityServer.csproj)");
            await cliService.RunAsync("powershell.exe", $"dotnet sln .\\{settings.Name}.sln remove (Get-ChildItem -r **/*.IdentityService.IdentityServer.csproj)");
            await cliService.RunAsync("powershell.exe", $"dotnet sln .\\{settings.Name}.sln remove (Get-ChildItem -r **/*.SaaSService.IdentityServer.csproj)");
            await cliService.RunAsync("powershell.exe", $"Remove-Item -Recurse -Force (Get-ChildItem -r **/*.SaaSService.IdentityServer)");
            await cliService.RunAsync("powershell.exe", $"Remove-Item -Recurse -Force (Get-ChildItem -r **/*.IdentityService.IdentityServer)");
            await cliService.RunAsync("powershell.exe", $"Remove-Item -Recurse -Force (Get-ChildItem -r **/*.AdministrationService.IdentityServer)");
            await cliService.RunAsync("powershell.exe", $"Remove-Item -Recurse -Force (Get-ChildItem -r **/*.MongoDB.Tests)");
            await cliService.RunAsync("powershell.exe", $"Remove-Item -Recurse -Force (Get-ChildItem -r **/*.MongoDB)");
            await cliService.RunAsync("powershell.exe", $"Remove-Item -Recurse -Force (Get-ChildItem -r **/*.Host.Shared)");
            await cliService.RunAsync("powershell.exe", $"Remove-Item -Recurse -Force (Get-ChildItem -r **/*.Installer)");
            await cliService.RunAsync("abp", $"add-module Volo.AuditLogging -s services\\administration\\{settings.Name}.AdministrationService.sln --skip-db-migrations");
            await cliService.RunAsync("abp", $"add-module Volo.FeatureManagement -s services\\administration\\{settings.Name}.AdministrationService.sln --skip-db-migrations");
            await cliService.RunAsync("abp", $"add-module Volo.PermissionManagement -s services\\administration\\{settings.Name}.AdministrationService.sln --skip-db-migrations");
            await cliService.RunAsync("abp", $"add-module Volo.SettingManagement -s services\\administration\\{settings.Name}.AdministrationService.sln --skip-db-migrations");
            await cliService.RunAsync("abp", $"add-module Volo.Identity -s services\\identity\\{settings.Name}.IdentityService.sln --skip-db-migrations");
            await cliService.RunAsync("abp", $"add-module Volo.IdentityServer -s services\\identity\\{settings.Name}.IdentityService.sln --skip-db-migrations");
            await cliService.RunAsync("abp", $"add-module Volo.TenantManagement -s services\\saas\\{settings.Name}.SaaSService.sln --skip-db-migrations");
            return 0;
        }

        private void MoveDirectory(string source, string target)
        {
            var sourcePath = source.TrimEnd('\\', ' ');
            var targetPath = target.TrimEnd('\\', ' ');
            var files = Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories)
                                 .GroupBy(s => Path.GetDirectoryName(s));
            foreach (var folder in files)
            {
                var targetFolder = folder.Key.Replace(sourcePath, targetPath);
                Directory.CreateDirectory(targetFolder);
                foreach (var file in folder)
                {
                    var targetFile = Path.Combine(targetFolder, Path.GetFileName(file));
                    if (File.Exists(targetFile)) File.Delete(targetFile);
                    File.Move(file, targetFile);
                }
            }
            Directory.Delete(source, true);
        }
    }
}