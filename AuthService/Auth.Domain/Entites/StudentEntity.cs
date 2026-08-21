namespace Auth.Domain.Entites
{
    public class StudentEntity
    {
        public Guid UserId { get; set; }
        public int EnrolledCoursesCount { get; set; }
        public int CompletedCoursesCount { get; set; }
        public int LearningHours { get; set; }
        public int CertificatesCount { get; set; }
        public int StreakDays { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public UserEntity User { get; set; }

    }
}
