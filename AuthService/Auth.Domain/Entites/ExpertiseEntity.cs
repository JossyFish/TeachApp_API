namespace Auth.Domain.Entites
{
    public class ExpertiseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<TeacherExpertiseEntity> Teachers { get; set; } = [];
    }
}
