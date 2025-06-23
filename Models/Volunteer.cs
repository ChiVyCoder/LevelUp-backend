using System;
using System.ComponentModel.DataAnnotations; // Để dùng [Key]

namespace LevelUp.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerID { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SkillsRequired { get; set; } = string.Empty;
        public DateTime DatePosted { get; set; }   
    }
}