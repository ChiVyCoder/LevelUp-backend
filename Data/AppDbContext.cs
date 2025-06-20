using LevelUp.Models;
using Microsoft.EntityFrameworkCore;

namespace LevelUp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<VolunteerApplication> VolunteerApplications { get; set; }
        public DbSet<InternshipRegistration> InternshipRegistrations { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().Ignore(u => u.Password);

            modelBuilder.Entity<UserCourse>()
                .HasIndex(uc => new { uc.UserId, uc.CourseId })
                .IsUnique();

            // Cấu hình mối quan hệ giữa InternshipRegistration và User
            modelBuilder.Entity<InternshipRegistration>()
                .HasOne(ir => ir.User)
                .WithMany(u => u.InternshipRegistrations)
                .HasForeignKey(ir => ir.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình mối quan hệ giữa InternshipRegistration và Job
            modelBuilder.Entity<InternshipRegistration>()
                .HasOne(ir => ir.Job)
                .WithMany(j => j.InternshipRegistrations)
                .HasForeignKey(ir => ir.JobID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InternshipRegistration>()
                .HasIndex(ir => new { ir.UserId, ir.JobID })
                .IsUnique();

            // Cấu hình VolunteerApplication
            modelBuilder.Entity<VolunteerApplication>()
                .HasOne(va => va.User)
                .WithMany()
                .HasForeignKey(va => va.UserId);
        }
    }
}