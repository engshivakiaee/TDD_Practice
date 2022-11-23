using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Processor;
using DeskBooker.DataAccess;
using DeskBooker.DataAccess.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


var connectionString = "DataSource=:memory:";
var connection = new SqliteConnection(connectionString);
connection.Open();

builder.Services.AddDbContext<DeskBookerContext>(x=>x.UseSqlite(connection));



builder.Services.AddTransient<IDeskBookingRepository,DeskBookingRepository>();
builder.Services.AddTransient<IDeskRepository,DeskRepository>();
builder.Services.AddTransient<IDeskBookingRequestProcessor, DeskBookingRequestProcessor>();

var app = builder.Build();

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
