using LevelUp.Data;
using LevelUp.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.Extensions.Logging;

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

string finalConnectionString;
try
{
    var npgsqlBuilder = new NpgsqlConnectionStringBuilder(rawConnectionString);
    finalConnectionString = npgsqlBuilder.ConnectionString;
}
catch (ArgumentException ex)
{
    // Đây là nơi lỗi xảy ra. Dòng này sẽ được ghi vào log của Render.
    // Lỗi ArgumentException chỉ ra chuỗi kết nối rawConnectionString không hợp lệ.
    Console.WriteLine($"Error parsing connection string: {ex.Message}");
    Console.WriteLine($"Raw Connection String: '{rawConnectionString}'");
    throw; // Ném lại lỗi để Render báo lỗi deploy
}


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