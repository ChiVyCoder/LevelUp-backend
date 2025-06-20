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
        public string Fullname { get; set; }
        public int UserId { get; set; } 
        [ForeignKey("UserId")]

        public int VolunteerID { get; set; } 
        [ForeignKey("VolunteerID")]
        public Volunteer? Volunteer { get; set; }
        public User? User { get; set; }
        public string Gender { get; set; } 
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Reason { get; set; } 
        public string Skills { get; set; } 
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
    }
}
