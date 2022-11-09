using ChatApplicationWithSQLServer;
using ChatApplicationWithSQLServer.Repositories;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services); // calling ConfigureServices method

//builder.Services.AddCors();
builder.Services.AddCors(o => o.AddDefaultPolicy(builder =>
{
	builder.AllowAnyOrigin()
				 .AllowAnyMethod()
				 .AllowAnyHeader();
}));

var app = builder.Build();

startup.Configure(app, builder.Environment); // calling Configure method

// Add services to the container.

builder.Services.AddControllersWithViews();
//builder.Services.AddControllers();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
//app.UseCors(x => x
//							.AllowAnyOrigin()
//							.AllowAnyMethod()
//							.AllowAnyHeader());

app.UseAuthorization();

app.MapHub<EchoHub>("/echo");
app.MapControllerRoute(
		name: "default",
		pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();

