using LevelUp.Data;
using LevelUp.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

// Lấy chuỗi kết nối
string rawConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

if (string.IsNullOrEmpty(rawConnectionString))
{
    rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

if (string.IsNullOrEmpty(rawConnectionString))
{
    throw new InvalidOperationException("Database connection string is not configured. Please set 'ConnectionStrings:DefaultConnection' in appsettings.json or 'DATABASE_URL' environment variable.");
}

string finalConnectionString;

if (rawConnectionString.StartsWith("postgres://") || rawConnectionString.StartsWith("postgresql://"))
{
    // Chuyển từ URI sang key=value format cho Npgsql
    var uri = new Uri(rawConnectionString);
    var userInfo = uri.UserInfo.Split(':');

    var builderNpgsql = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port,
        Username = userInfo[0],
        Password = userInfo[1],
        Database = uri.AbsolutePath.Trim('/'),
        SslMode = SslMode.Require,
        TrustServerCertificate = true
    };

    finalConnectionString = builderNpgsql.ConnectionString;
}
else
{
    // Đã là key=value sẵn
    finalConnectionString = rawConnectionString;
}

// Đăng ký DbContext
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

// Tạo database nếu chưa có
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
