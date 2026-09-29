using Auth.Domain.Enums;

namespace Auth.Domain.Entites
{
    public class SubscriptionPlanEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public SubscriptionPlanCode Code { get; set; }    
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public string? Description { get; set; }
        public bool IsPopular { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<TeacherEntity> Teachers { get; set; } = [];
    }
}
