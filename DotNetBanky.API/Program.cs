using DotNetBanky.Common.DIContainer;
using DotNetBanky.Core.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddBankyDatabase(builder.Configuration);
builder.Services.AddBankyRepositories();

builder.Services.AddBankyIdentity(RoleConstants.Admin, RoleConstants.Customer, RoleConstants.CashierAndAbove);
builder.Services.AddJwt(builder.Configuration);

builder.Services.AddBankyServices(builder.Configuration);
builder.Services.AddAzureSearch(builder.Configuration);
builder.Services.RegisterAutoMapper();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
