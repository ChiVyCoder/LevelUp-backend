using LevelUp.Models;
using System.ComponentModel.DataAnnotations;

public class Job
{
    [Key]
    public int JobID { get; set; }
    public string? LogoURL { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? Salary { get; set; }
    public string? Industry { get; set; }
    public string? JobType { get; set; }
    public string? Tooltip { get; set; }
    public ICollection<InternshipRegistration> InternshipRegistrations { get; set; } = new List<InternshipRegistration>();
}
