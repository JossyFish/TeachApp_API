namespace Auth.Domain.Entites
{
    public class TeacherEntity
    {
        public Guid UserId { get; set; }
        public string Expertise { get; set; }
        public string Experience { get; set; }
        public string Bio { get; set; }
        public string SubscriptionPlan { get; set; }
        public DateTime SubscriptionExpiresAt { get; set; } = DateTime.UtcNow;
        public string CardLastDigits { get; set; }
        public bool IsActive { get; set; } = true;
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int AvgRating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public UserEntity User { get; set; }
    }
}
