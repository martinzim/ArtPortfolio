using Oqtane.Extensions;
using Oqtane.Infrastructure;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Oqtane services configuration
builder.Services.AddOqtane(builder.Configuration, builder.Environment);

var app = builder.Build();

app.MapDefaultEndpoints();

// Oqtane middleware - resolve required services from DI
var configuration = app.Services.GetRequiredService<IConfigurationRoot>();
var environment = app.Services.GetRequiredService<IWebHostEnvironment>();
var corsService = app.Services.GetRequiredService<ICorsService>();
var corsPolicyProvider = app.Services.GetRequiredService<ICorsPolicyProvider>();
var syncManager = app.Services.GetRequiredService<ISyncManager>();

app.UseOqtane(configuration, environment, corsService, corsPolicyProvider, syncManager);

await app.RunAsync();
