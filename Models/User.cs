// LevelUp/Models/User.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace LevelUp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên người dùng là bắt buộc.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        public string Password { get; set; }

        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }

        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; } = "Chưa có mô tả cá nhân.";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
        public ICollection<InternshipRegistration> InternshipRegistrations { get; set; } = new List<InternshipRegistration>();
        public ICollection<VolunteerRegistration> VolunteerRegistrations { get; set; } = new List<VolunteerRegistration>();
    }

    public class Login
    {
        [Required]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        public string Password { get; set; }
    }

    public class UserProfileUpdateDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
    }
}