using DotNetBanky.Common.AutomatedMigrations;
using DotNetBanky.Common.DIContainer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddBankyDatabase(builder.Configuration);

builder.Services.AddBankyServices(builder.Configuration);

builder.Services.RegisterAutoMapper();

builder.Services.AddBankyIdentity();

builder.Services.AddRazorPages();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
