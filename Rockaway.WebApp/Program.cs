var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

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

app.MapGet("/hello/{name}", (string name) => new Customer(name, "Aardvark"));


app.Run();


public class Customer(string firstName, string lastName) {
	public string FirstName { get; set; } = firstName;
	public string LastName { get; set; } = lastName;
}
