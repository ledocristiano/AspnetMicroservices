using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

var logging = builder.Logging;

logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
logging.AddConsole();
logging.AddDebug();

var services = builder.Services;

services.AddOcelot()
    .AddCacheManager(settings => settings.WithDictionaryHandle());

var app = builder.Build();

app.UseRouting();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
