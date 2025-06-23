namespace LevelUp.Models
{
    public class VolunteerOrganization
    {
        public int VolunteerOrganizationId { get; set; }
        public string OrganizationName { get; set; } = null!;
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }

        public ICollection<VolunteerRegistration> VolunteerRegistrations { get; set; } = new List<VolunteerRegistration>();
    }
}
