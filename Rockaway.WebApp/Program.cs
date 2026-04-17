using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Rockaway.RazorComponents.Components;
using Rockaway.WebApp.Components;
using Rockaway.WebApp.Data;
using Rockaway.WebApp.Hosting;
using Rockaway.WebApp.Services;
using Rockaway.WebApp.Services.Mail;
using System.Net;
using Microsoft.Build.Experimental;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddRazorPages();

builder.Services.AddRazorPages(options
	=> options.Conventions.AuthorizeAreaFolder("admin", "/"));

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IReportServerStatus>(new StatusReporter());
builder.Services.AddSingleton<IClock>(SystemClock.Instance);

#if DEBUG && !NCRUNCH
builder.Services.AddSassCompiler();
#endif

var logger = CreateAdHocLogger<Program>()!;

logger.LogInformation("Rockaway running in {environment} environment", builder.Environment.EnvironmentName);

if (builder.Environment.ShouldUseSqlite) {
	logger.LogInformation("Using Sqlite database");
	var sqliteConnection = new SqliteConnection("Data Source=:memory:");
	sqliteConnection.Open();
	builder.Services.AddDbContext<RockawayDbContext>(options => options.UseSqlite(sqliteConnection));
} else {
	logger.LogInformation("Using SQL Server database");
	var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
	builder.Services.AddDbContext<RockawayDbContext>(options => options.UseSqlServer(connectionString));
}

builder.Services.AddDefaultIdentity<IdentityUser>()
	.AddEntityFrameworkStores<RockawayDbContext>();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();

builder.Services.AddHostedService<TicketMailerBackgroundService>();

// Add this to Program.cs

builder.Services.AddSingleton<IMailSender, SmtpMailSender>();
var smtpSettings = new SmtpSettings();
builder.Configuration.Bind("Smtp", smtpSettings);
builder.Services.AddSingleton(smtpSettings);
builder.Services.AddSingleton<ISmtpRelay, SmtpRelay>();

builder.Services.AddSingleton<IMailQueue>(new TicketOrderMailQueue());

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
	using var db = scope.ServiceProvider.GetService<RockawayDbContext>()!;
	if (app.Environment.ShouldUseSqlite) {
		db.Database.EnsureCreated();
	} else if (Boolean.TryParse(app.Configuration["apply-migrations"], out var applyMigrations) && applyMigrations) {
		logger.LogInformation("apply-migrations=true was specified. Applying EF migrations and then exiting.");
		db.Database.Migrate();
		logger.LogInformation("EF database migrations applied successfully.");
		Environment.Exit(0);
	}
}

if (!app.Environment.ShouldDisplayDetailedErrors) app.UseExceptionHandler("/Error");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {

	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapAreaControllerRoute(
	name: "admin",
	areaName: "Admin",
	pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
).RequireAuthorization();

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

app.MapRazorComponents<TicketPicker>()
	.AddInteractiveServerRenderMode()
	.AddInteractiveWebAssemblyRenderMode();

app.Run();
return;

ILogger<T> CreateAdHocLogger<T>()
	=> LoggerFactory.Create(lb => lb.AddConsole()).CreateLogger<T>();