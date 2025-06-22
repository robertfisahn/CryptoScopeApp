using Microsoft.EntityFrameworkCore;
using MediatR;
using CryptoScopeAPI.Services;
using CryptoScopeAPI.Features.GetCoins;
using System.Reflection;
using CryptoScopeAPI.Mappings;
using CryptoScopeAPI.Dtos;
using CryptoScopeAPI.Features.GetSearchCoin;
using CryptoScopeAPI.Features.GetCoinDetails;
using CryptoScopeAPI.Features.GetCoinMarketChart;
using CryptoScopeAPI;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error
    )
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ICoinGeckoClient, CoinGeckoClient>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "CryptoScopeApp");
});
builder.Services.AddAutoMapper(typeof(CoinMappingProfile));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=coins.db"));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddHostedService<CoinListSyncService>();
builder.Services.AddHostedService<SearchCoinSyncService>();

builder.Services.Configure<CoinSyncSettings>(
    builder.Configuration.GetSection("CoinSync"));
var app = builder.Build();

app.UseCors();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapGet("/api/health", () => Results.Ok("Healthy!"));

app.MapGet("/api/coins", async (IMediator mediator) =>
{
    var result = await mediator.Send(new GetCoinsQuery());
    return Results.Ok(result);
});

app.MapGet("/api/coins/search", async (IMediator mediator) =>
{
    var searchCoins = await mediator.Send(new GetSearchCoinQuery());
    return Results.Ok(searchCoins);
});

app.MapGet("/api/coins/{id}", async (string id, IMediator mediator) =>
{
    var result = await mediator.Send(new GetCoinDetailsQuery(id));
    return Results.Ok(result);
});

app.MapGet("/api/coins/{id}/market_chart", async (string id, string days, IMediator mediator) =>
{
    var result = await mediator.Send(new GetCoinMarketChartQuery(id, days));
    return Results.Ok(result);
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();