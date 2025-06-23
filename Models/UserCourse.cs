namespace LevelUp.Models
{
    public class UserCourse
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = new User();

        public int CourseId { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.Now;
        public bool Completed { get; set; } = false;
    }
}