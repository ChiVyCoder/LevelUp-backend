using LevelUp.Models;
using System.ComponentModel.DataAnnotations;
namespace LevelUp.Models
{
    public class InternshipCompany
    {
        [Key]
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? Location { get; set; }
        public ICollection<InternshipRegistration> InternshipRegistrations { get; set; } = new List<InternshipRegistration>();
    }
}
