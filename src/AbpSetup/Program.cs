using AbpSetup.Commands;
using AbpSetup.Infrastructure;
using AbpSetup.Services;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var registrations = new ServiceCollection();
registrations.AddTransient<CliService>();

var registrar = new TypeRegistrar(registrations);

var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.AddCommand<CreateCommand>("create");
});

return app.Run(args);