using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Rockaway.WebApp.Data;
using Rockaway.WebApp.Hosting;
using Rockaway.WebApp.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IReportServerStatus>(new StatusReporter());

var logger = CreateAdHocLogger<Program>()!;

logger.LogInformation("Rockaway running in {environment} environment", builder.Environment.EnvironmentName);
if (builder.Environment.UseSqlite()) {
	logger.LogInformation("Using Sqlite database");
	var sqliteConnection = new SqliteConnection("Data Source=:memory:");
	sqliteConnection.Open();
	builder.Services.AddDbContext<RockawayDbContext>(options => options.UseSqlite(sqliteConnection));
} else {
	logger.LogInformation("Using SQL Server database");
	var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
	builder.Services.AddDbContext<RockawayDbContext>(options => options.UseSqlServer(connectionString));
}

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
	using (var db = scope.ServiceProvider.GetService<RockawayDbContext>()!) {
		db.Database.EnsureCreated();
	}
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/status", (IReportServerStatus reporter)
	=> reporter.GetStatus());

app.MapGet("/hello", () => TypedResults.Content("""
                                                <!DOCTYPE html>
                                                <html>
                                                <head><title>Hey!</title></head>
                                                <body>Hey!</body>
                                                </html>
                                                """,
	contentType: "text/html",
	statusCode: (int?) HttpStatusCode.OK));

app.Run();
return;

ILogger<T> CreateAdHocLogger<T>()
	=> LoggerFactory.Create(lb => lb.AddConsole()).CreateLogger<T>();