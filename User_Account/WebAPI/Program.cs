using Application;
using Infrastructure;
using Persistence;
using Serilog;
using WebAPI;
using WebAPI.Checks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddHealthCheckService(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddWebAPI();

builder.Services.AddControllers();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();
app.AddUpdateMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.AddHealthCheckApp();

app.MapControllers();

app.Run();