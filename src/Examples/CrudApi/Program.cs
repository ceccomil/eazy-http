global using CaptainLogger;
global using CrudApi.Helpers;
global using Microsoft.AspNetCore.Mvc;
global using CrudApi.Services;
global using LiteDB;
global using CrudApi.Dtos;
global using Microsoft.AspNetCore.Mvc.Filters;
global using CrudApi.Authentication;

var builder = WebApplication
    .CreateBuilder(args);

builder
    .Logging
    .ConfigureLogging();

builder
    .Services
    .AddControllers();

builder
    .Services
    .AddEndpointsApiExplorer()
    .AddScoped<IDbService, DbService>();


var app = builder
    .Build();

app
    .UseHttpsRedirection()
    .UseAuthorization();

app
    .MapControllers();

app
    .Run();
