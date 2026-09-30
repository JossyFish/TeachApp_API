namespace Auth.Domain.Models.Models
{
    public class Teacher
    {
        public Guid UserId { get; set; }
        public List<int> ExpertiseIds { get; set; } = new();
        public string Experience { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public int SubscriptionPlanId { get; set; }
        public DateTime SubscriptionExpiresAt { get; set; }
        public string CardLastDigits { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int AvgRating { get; set; }

        public Teacher() { }

        public Teacher(Guid userId, List<int> expertiseIds, string experience, string bio,
            int subscriptionPlanId, DateTime subscriptionExpiresAt, string cardLastDigits, 
            bool isActive = true, int totalStudents = 0, int totalCourses = 0, int avgRating = 0)
        {
            UserId = userId;
            ExpertiseIds = expertiseIds;
            Experience = experience;
            Bio = bio;
            SubscriptionPlanId = subscriptionPlanId;
            SubscriptionExpiresAt = subscriptionExpiresAt;
            CardLastDigits = cardLastDigits;
            IsActive = isActive;
            TotalStudents = totalStudents;
            TotalCourses = totalCourses;
            AvgRating = avgRating;
        }
    }
}
