using System;
using System.ComponentModel.DataAnnotations; // Để dùng [Key]

namespace LevelUp.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerID { get; set; }
        public string ImageURL { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Industry { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string SkillsRequired { get; set; }
        public DateTime DatePosted { get; set; } 
    }
}