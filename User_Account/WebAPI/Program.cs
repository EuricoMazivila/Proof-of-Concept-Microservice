using Application;
using Infrastructure;
using Persistence;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddPersistence(builder.Configuration)
    .AddInfrastructure()
    .AddApplication()
    .AddWebAPI();

builder.Services.AddControllers();

var app = builder.Build();
app.AddUpdateMigrations();

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