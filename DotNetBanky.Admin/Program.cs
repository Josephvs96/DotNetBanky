using AspNetCoreHero.ToastNotification;
using DotNetBanky.Common.AutomatedMigrations;
using DotNetBanky.Common.DIContainer;
using Microsoft.AspNetCore.Authorization;
using SmartBreadcrumbs.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddBankyDatabase(builder.Configuration);

builder.Services.AddBankyRepositories();

builder.Services.AddBankyServices(builder.Configuration);

builder.Services.AddAzureSearch(builder.Configuration);

builder.Services.RegisterAutoMapper();

builder.Services.AddBankyIdentity();

builder.Services.AddBreadcrumbs(Assembly.GetExecutingAssembly());

builder.Services.AddNotyf(options =>
{
    options.DurationInSeconds = 3;
    options.IsDismissable = true;
    options.HasRippleEffect = false;
    options.Position = NotyfPosition.BottomRight;
});


builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/Dashboard/Index", "");
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
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

app.MapRazorPages();

app.Run();
