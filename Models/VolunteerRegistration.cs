namespace LevelUp.Models
{
    public class VolunteerRegistration
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int VolunteerOrganizationId { get; set; }
        public VolunteerOrganization Organization { get; set; }

        public int VolunteerID { get; set; }

        public Volunteer Volunteer { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.Now;
    }
}
