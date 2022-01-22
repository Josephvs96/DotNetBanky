using DotNetBanky.BLL.Services;
using DotNetBanky.Common.DIContainer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder();

host.ConfigureDefaults(args);

host.ConfigureServices((context, services) =>
{
    services.AddBankyDatabase(context.Configuration);
    services.AddBankyServices(context.Configuration);
    services.AddBankyRepositories();
});

var app = host.Build();

using (var scope = app.Services.CreateScope())
{
    var fraudService = scope.ServiceProvider.GetRequiredService<IFraudService>();
    await fraudService.StartFraudScan();
}

Console.WriteLine("Scan complete! exiting...");
