namespace Auth.Domain.Entites
{
    public class TeacherExpertiseEntity
    {
        public Guid TeacherId { get; set; }
        public int ExpertiseId { get; set; } 

        public TeacherEntity Teacher { get; set; } = null!;
        public ExpertiseEntity Expertise { get; set; } = null!;
    }
}
