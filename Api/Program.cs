
using AL;
using BLL;
using Castle.Windsor;
using DAL;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Castle.Windsor.Installer;
using Castle.Windsor.MsDependencyInjection;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Api;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHealthChecks();

builder.Services.AddScoped<IYahooAPIService, YahooAPIService>();
builder.Services.AddScoped<ITickerService, TickerService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<StockhistoryContextProcedures, StockhistoryContextProcedures>();

builder.Services.AddHostedService<StartupBackgroundService>();
builder.Services.AddSingleton<StartupHealthCheck>();
builder.Services.AddSingleton<SqlConnectionHealthCheck>();
builder.Services.AddHealthChecks()
    .AddCheck<StartupHealthCheck>(
        "Startup",
        tags: new[] { "ready" });

builder.Services.AddDbContext<StockInfoDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("StockHistoryDatabase")));
builder.Services.AddDbContext<StockhistoryContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("StockHistoryDatabase")));

builder.Services.AddHealthChecksUI().AddInMemoryStorage();


var app = builder.Build();


app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("ready")
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseHealthChecksUI(config => config.UIPath = "/hc-ui");



app.UseRouting().UseEndpoints(endpoints =>
{
    //...
    endpoints.MapHealthChecks("/hc");
    
});


app.MapControllers();



app.Run();

