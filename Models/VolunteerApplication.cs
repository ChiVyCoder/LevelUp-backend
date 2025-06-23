// LevelUp/Models/VolunteerApplication.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LevelUp.Models
{
    public class VolunteerApplication
    {
        [Key]
        public int Id { get; set; }

        public string Fullname { get; set; } = string.Empty; 

        public int UserId { get; set; }
        // [ForeignKey("UserId")] // XÓA DÒNG NÀY

        public int VolunteerID { get; set; }
        public Volunteer? Volunteer { get; set; }

        public User? User { get; set; }

        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Skills { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
    }
}