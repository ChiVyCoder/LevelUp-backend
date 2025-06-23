namespace LevelUp.Models
{
    public class VolunteerRegistration
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = new User();

        public int VolunteerOrganizationId { get; set; }

        public int VolunteerID { get; set; }

        public  ICollection<Volunteer> Volunteers { get; set; } = new List<Volunteer>();

        public DateTime RegisteredAt { get; set; } = DateTime.Now;
    }
}
