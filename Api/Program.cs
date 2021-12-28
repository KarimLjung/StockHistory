
using AL;
using BLL;
using Castle.Windsor;
using DAL;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IYahooAPIService, YahooAPIService>();
builder.Services.AddScoped<ITickerService, TickerService>();

builder.Services.AddDbContext<StockInfoDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("StockHistoryDatabase")));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
