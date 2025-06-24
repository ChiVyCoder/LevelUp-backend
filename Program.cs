using LevelUp.Data;
using LevelUp.Models;
using LevelUp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
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
        Port = uri.Port > 0 ? uri.Port : 5432,
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
            policy.WithOrigins("https://levelup-ui.vercel.app", "http://localhost:3000")
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
    // --- BẮT ĐẦU PHẦN THÊM DỮ LIỆU JOB ---
    if (!context.Jobs.Any()) // Chỉ chèn nếu bảng Jobs rỗng
    {
        var jobsToSeed = new List<Job>
        {
            new Job { LogoURL = "https://ibrand.vn/wp-content/uploads/2024/08/vinamilk-logo_brandlogos.net_quayf.png", CompanyName = "Vinamilk", Tooltip = "Học vấn: Cao đẳng\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Nhân viên kế toán\nKinh nghiệm: 1-3 năm ", Location = "Hồ Chí Minh", Salary = "15 triệu", Industry = "Kinh tế", JobType = "Full-time" },
            new Job { LogoURL = "https://icolor.vn/wp-content/uploads/2024/08/logo-vinfast-2.png", CompanyName = "VinFast", Tooltip = "Học vấn: Đại học\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Trưởng phòng marketing\nKinh nghiệm: 1-3 năm", Location = "Hà Nội", Salary = "15 triệu", Industry = "Marketing", JobType = "Full-time" },
            new Job { LogoURL = "https://forbes.vn/wp-content/uploads/2022/08/LogoTop25fnb_masanconsumer.jpg", CompanyName = "Masan Consumer", Tooltip = "Học vấn: Cao đẳng\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Nhân viên thiết kế\nKinh nghiệm: 6 tháng", Location = "Hà Nội", Salary = "10 triệu", Industry = "Thiết kế", JobType = "Full-time" },
            new Job { LogoURL = "https://i.pinimg.com/736x/4a/f4/32/4af432166870bb454a4141a0ebbdcbcf.jpg", CompanyName = "Milo", Tooltip = "Học vấn: Đại học\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Nhân viên sáng tạo nội dung\nKinh nghiệm: 6 tháng", Location = "Đà Nẵng", Salary = "10 triệu", Industry = "Sáng tạo nội dung", JobType = "Full-time" },
            new Job { LogoURL = "https://static.wixstatic.com/media/9d8ed5_810e9e3b7fad40eca3ec5087da674662~mv2.png/v1/fill/w_560,h_560,al_c,q_85,usm_0.66_1.00_0.01,enc_avif,quality_auto/9d8ed5_810e9e3b7fad40eca3ec5087da674662~mv2.png", CompanyName = "Vietcombank", Tooltip = "Học vấn: Cao đẳng\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Nhân viên bảo mật\nKinh nghiệm: Trên 3 năm", Location = "Hà Nội", Salary = "20 triệu", Industry = "CNTT", JobType = "Part-time" },
            new Job { LogoURL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSzCZn-leAnYIeMivjXCm2bPuFhwEaJKl5VyQ&s", CompanyName = "PS", Tooltip = "Học vấn: Không yêu cầu\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Nhân viên marketing\nKinh nghiệm: 1-3 năm", Location = "Hà Nội", Salary = "15 triệu", Industry = "Marketing", JobType = "Full-time" },
            new Job { LogoURL = "https://brandlogos.net/wp-content/uploads/2022/10/coca-cola-logo_brandlogos.net_8kh4z.png", CompanyName = "Coca Cola", Tooltip = "Học vấn: Đại học\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Nhân viên sáng tạo nội dung\nKinh nghiệm: 6 tháng", Location = "Hà Nội", Salary = "15 triệu", Industry = "Sáng tạo nội dung", JobType = "Full-time" },
            new Job { LogoURL = "https://royalhelmet.com.vn/storage/2024/08/logo-royal.png", CompanyName = "Royal Helmet VN", Tooltip = "Học vấn: Không yêu cầu\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Nhân viên thiết kế\nKinh nghiệm: 1 - 3 năm", Location = "Hồ Chí Minh", Salary = "10 triệu", Industry = "Thiết kế", JobType = "Part-time" },
            new Job { LogoURL = "https://gigamall.com.vn/data/2021/03/26/15385835_LOGO-VIETTIEN-900X900_thumbnail.png", CompanyName = "Viettien", Tooltip = "Học vấn: Đại học\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Trưởng phòng thiết kế\nKinh nghiệm: 1 năm", Location = "Cần Thơ", Salary = "20 triệu", Industry = "Thiết kế", JobType = "Full-time" },
            new Job { LogoURL = "https://d1yjjnpx0p53s8.cloudfront.net/styles/logo-thumbnail/s3/032018/untitled-1_333.png?_W.Zis6.eC2ARGfQeLo30s_zRnC53eVf&itok=eum04KYs", CompanyName = "Ensure", Tooltip = "Học vấn: Đại học\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Nhân viên lập trình\nKinh nghiệm: Trên 3 năm", Location = "Long An", Salary = "20 triệu", Industry = "CNTT", JobType = "Full-time" },
            new Job { LogoURL = "https://career.fpt-software.com/wp-content/themes/jobcareer/fpt_landing_page/taste-vietnam/images/user/11125/Logo_fpt_software.png", CompanyName = "FPT Software", Tooltip = "Học vấn: Không yêu cầu\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Trưởng phòng bảo mật hệ thống\nKinh nghiệm: Trên 3 năm", Location = "Hà Nội", Salary = "25 triệu", Industry = "CNTT", JobType = "Part-time" },
            new Job { LogoURL = "https://toppng.com/uploads/preview/mobifone-vector-logo-115742074208scmjpp01h.png", CompanyName = "Mobifone", Tooltip = "Học vấn: Cao đẳng/Đại học\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Nhân viên lập trình\nKinh nghiệm: 1-3 năm", Location = "Hà Nội", Salary = "20 triệu", Industry = "CNTT", JobType = "Full-time" },
            new Job { LogoURL = "https://yt3.googleusercontent.com/fo-sKLrPTLPi4rM5213vmFxZSd5BpJ6rVDrnWjlqFn3nAa0jFIXqPUK7c6RkoZigUQACW5xqPhY=s900-c-k-c0x00ffffff-no-rj", CompanyName = "VNG", Tooltip = "Học vấn: Cao đẳng/Đại học\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Nhân viên lập trình\nKinh nghiệm: 6 tháng", Location = "Hồ Chí Minh", Salary = "15 triệu", Industry = "CNTT", JobType = "Full-time" },
            new Job { LogoURL = "https://pngate.com/wp-content/uploads/2025/03/toyota-logo-main-1.png", CompanyName = "Toyota", Tooltip = "Học vấn: Đại học\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Quản lý phòng marketing\nKinh nghiệm: 1 - 3 năm", Location = "Hà Nội", Salary = "20 triệu", Industry = "Marketing", JobType = "Full-time" },
            new Job { LogoURL = "https://i.pinimg.com/736x/85/25/96/8525967ea6bb77e5cc38cd39ae9df34c.jpg", CompanyName = "Ganier", Tooltip = "Học vấn: Cao đẳng/Đại học\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Trưởng phòng kế toán\nKinh nghiệm: 6 tháng", Location = "Hà Nội", Salary = "10 triệu", Industry = "Kinh tế", JobType = "Full-time" },
            new Job { LogoURL = "https://mms.img.susercontent.com/43977fd1dbfeb256093b2c793356bae8", CompanyName = "DHC", Tooltip = "Học vấn: Không yêu cầu\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Nhân viên sáng tạo nội dung\nKinh nghiệm: 1-3 năm", Location = "Hà Nội", Salary = "10 triệu", Industry = "Sáng tạo nội dung", JobType = "Part-time" },
            new Job { LogoURL = "https://images.seeklogo.com/logo-png/39/2/viettel-fc-logo-png_seeklogo-397950.png", CompanyName = "Viettel", Tooltip = "Học vấn: Cao đẳng/Đại học\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Trưởng phòng bảo mật hệ thống\nKinh nghiệm: Trên 3 năm", Location = "Hà Nội", Salary = "25 triệu", Industry = "CNTT", JobType = "Full-time" },
            new Job { LogoURL = "https://cdn.haitrieu.com/wp-content/uploads/2021/11/Logo-The-Gioi-Di-Dong-MWG.png", CompanyName = "Thế Giới Di Động", Tooltip = "Học vấn: Cao đẳng/Đại học\nYêu cầu bằng cấp: Ielts 6.0\nVị trí/Cấp bậc: Nhân viên kế toán\nKinh nghiệm: 1 - 3 năm", Location = "Hà Nội", Salary = "15 triệu", Industry = "Kinh tế", JobType = "Full-time" }
        };
        context.Jobs.AddRange(jobsToSeed);
        context.SaveChanges();
    }
    if (!context.Volunteers.Any()) // Chỉ chèn nếu bảng Volunteers rỗng
    {
        var volunteersToSeed = new List<Volunteer>
        {
            new Volunteer { ImageURL = "https://karaoke.com.vn/wp-content/uploads/2018/03/29472261_431994757257423_4847596387174449152_o-300x225.jpg", Title = "Xây cầu từ thiện", Location = "Cần Thơ", Industry = "Xây dựng", Type = "Phi chính phủ", Description = "Chung tay xây dựng cầu mới cho người dân thuận tiện đi lại", SkillsRequired = "Không yêu cầu", DatePosted = new DateTime(2025, 7, 1) },
            new Volunteer { ImageURL = "https://ums.vnu.edu.vn/files/uploads/2024/02/123123.jpg", Title = "Hiến máu nhân đạo", Location = "TP.Hồ Chí Minh", Industry = "Y tế", Type = "Thuộc chính phủ", Description = "Hiến máu cho bệnh nhân nghèo", SkillsRequired = "Không yêu yêu cầu", DatePosted = new DateTime(2025, 8, 15) },
            new Volunteer { ImageURL = "https://icdn.dantri.com.vn/thumb_w/680/dansinh/2025/01/15/che-do-cua-giao-vien-day-hoc-o-mien-nui-122701134842-1736927785679.jpg", Title = "Dạy học vùng sâu", Location = "Hà Nội", Industry = "Giáo dục", Type = "Thuộc chính phủ", Description = "Nâng cao kiến thức trẻ em", SkillsRequired = "Bằng sư phạm GDTH", DatePosted = new DateTime(2025, 9, 6) },
            new Volunteer { ImageURL = "https://images2.thanhnien.vn/528068263637045248/2023/2/27/don-rac-thai-nhua-3-16774747323591124101879.jpg", Title = "Thu gom rác thải - nhựa", Location = "TP.Hồ Chí Minh", Industry = "Môi trường", Type = "Phi chính phủ", Description = "Dòng sông trên đường 4f bị ô nhiễm khá nặng", SkillsRequired = "Không yêu cầu", DatePosted = new DateTime(2025, 6, 17) },
            new Volunteer { ImageURL = "https://baocantho.com.vn/image/fckeditor/upload/2018/20180616/images/khambenhmienphigiaixuan.jpg", Title = "Khám bệnh cho người nghèo", Location = "An Giang", Industry = "Y tế", Type = "Thuộc chính phủ", Description = "Vì một tương lai cho sự khoẻ mạnh", SkillsRequired = "Bằng Y-Dược", DatePosted = new DateTime(2025, 11, 5) },
            new Volunteer { ImageURL = "https://images.hcmcpv.org.vn/res/news/2022/03/20-03-2022-quan-1-ra-quan-ngay-chu-nhat-xanh-F953BA66.jpg", Title = "Chủ Nhật Xanh", Location = "Cần Thơ", Industry = "Môi trường", Type = "Phi chính phủ", Description = "Tổ chức buổi tổng vệ sinh môi trường học tập", SkillsRequired = "Không yêu cầu", DatePosted = new DateTime(2025, 8, 12) },
            new Volunteer { ImageURL = "https://bna.1cdn.vn/2020/10/25/uploaded-myhabna-2020_10_25-_bna_image_6029069_25102020.jpeg", Title = "Quyên góp đồ dùng học tập", Location = "Hà Nội", Industry = "Giáo dục", Type = "Phi chính phủ", Description = "Hỗ trợ chia sẻ đồ dùng học tập", SkillsRequired = "Không yêu cầu", DatePosted = new DateTime(2025, 8, 1) },
            new Volunteer { ImageURL = "https://bcp.cdnchinhphu.vn/334894974524682240/2024/9/17/bhyt-17265619815172023734832.jpg", Title = "Hỗ trợ bệnh nhân khám bệnh", Location = "TP.Hồ Chí Minh", Industry = "Y tế", Type = "Thuộc chính phủ", Description = "Giúp đỡ các bệnh nhân đăng kí khám bệnh", SkillsRequired = "Không yêu cầu", DatePosted = new DateTime(2025, 2, 15) },
            new Volunteer { ImageURL = "https://media.baoquangninh.vn/dataimages/202005/original/images1392560_04.jpg", Title = "Xây mái ấm tình thương", Location = "An Giang", Industry = "Xây dựng", Type = "Phi chính phủ", Description = "Giúp các em có mái nhà để về", SkillsRequired = "Không yêu cầu", DatePosted = new DateTime(2021, 7, 15) },
            new Volunteer { ImageURL = "https://c1nguyendu.pgdhadong.edu.vn/uploads/thnguyendu-hd/news/2022_09/image-20220925210553-1.jpeg", Title = "Tuyên truyền pháp luật ATGT", Location = "Hà Nội", Industry = "Giáo dục", Type = "Thuộc chính phủ", Description = "Nâng cao ý thức về an toàn giao thông", SkillsRequired = "Không yêu cầu", DatePosted = new DateTime(2025, 8, 19) },
            new Volunteer { ImageURL = "https://image.baodauthau.vn/w750/Uploaded/2025/qjmfn/2025_03_06/000-1-1016-4361.jpg", Title = "Sửa chữa đường lộ", Location = "Cần Thơ", Industry = "Xây dựng", Type = "Thuộc chính phủ", Description = "Khôi phục con đường làng nâng cao chất lượng đời sống", SkillsRequired = "Không yêu cầu", DatePosted = new DateTime(2026, 2, 15) },
            new Volunteer { ImageURL = "https://hiu.vn/wp-content/uploads/2020/04/ai.jpg", Title = "Nâng cao kiến thức về AI", Location = "TP.Hồ Chí Minh", Industry = "Giáo dục", Type = "Phi chính phủ", Description = "Bổ sung kiến thức về ngành Trí Tuệ Nhân Tạo cho sinh viên", SkillsRequired = "Bằng AI", DatePosted = new DateTime(2025, 10, 15) }
        };
        context.Volunteers.AddRange(volunteersToSeed);
        context.SaveChanges();
    }
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
