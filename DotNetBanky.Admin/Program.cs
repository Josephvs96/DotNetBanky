using DotNetBanky.Common.AutomatedMigrations;
using DotNetBanky.Common.DIContainer;
using DotNetBanky.Core.Constants;
using NToastNotify;
using SmartBreadcrumbs.Extensions;
using System.Globalization;
using System.Reflection;

ConfigureCulture();

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddBankyDatabase(builder.Configuration);

builder.Services.AddBankyRepositories();

builder.Services.AddBankyServices(builder.Configuration);

builder.Services.AddAzureSearch(builder.Configuration);

builder.Services.RegisterAutoMapper();

builder.Services.AddBankyIdentity(RoleConstants.Admin, RoleConstants.Cahsier);

builder.Services.AddBreadcrumbs(Assembly.GetExecutingAssembly());


builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/Dashboard/Index", "");
}).AddNToastNotifyToastr(new ToastrOptions
{
    PositionClass = ToastPositions.BottomRight,
    ProgressBar = true,
    CloseButton = true,
    ShowDuration = 4
});

builder.Services.AddResponseCaching();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await DbInitilizer.MigrateAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseResponseCaching();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseNToastNotify();

app.MapRazorPages();

app.Run();

void ConfigureCulture()
{
    var cultureInfo = new CultureInfo("sv-SE");
    cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
    cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
    cultureInfo.NumberFormat.NumberGroupSeparator = ",";
    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
}