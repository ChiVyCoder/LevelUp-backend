using LevelUp.Data;
using LevelUp.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

string rawConnectionString;

rawConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

if (string.IsNullOrEmpty(rawConnectionString))
{
    rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

if (string.IsNullOrEmpty(rawConnectionString))
{
    throw new InvalidOperationException("Database connection string is not configured. Please set 'ConnectionStrings:DefaultConnection' in appsettings.json or 'DATABASE_URL' environment variable.");
}

var npgsqlBuilder = new NpgsqlConnectionStringBuilder(rawConnectionString);
string finalConnectionString = npgsqlBuilder.ConnectionString;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(finalConnectionString));

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("https://levelup-ui.vercel.app")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddAuthorization();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();