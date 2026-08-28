namespace Auth.Domain.Models
{
    public class Student
    {
        public Guid UserId { get; set; }
        public int EnrolledCoursesCount { get; set; }
        public int CompletedCoursesCount { get; set; }
        public int LearningHours { get; set; }
        public int CertificatesCount { get; set; }
        public int StreakDays { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User User { get; set; }


        public Student(Guid userId, int enrolledCoursesCount = 0, int completedCoursesCount = 0,
            int learningHours = 0, int certificatesCount = 0, int streakDays = 0)
        {
            UserId = userId;
            EnrolledCoursesCount = enrolledCoursesCount;
            CompletedCoursesCount = completedCoursesCount;
            LearningHours = learningHours;
            CertificatesCount = certificatesCount;
            StreakDays = streakDays;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }


    }
}
