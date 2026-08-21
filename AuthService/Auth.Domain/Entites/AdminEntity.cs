namespace Auth.Domain.Entites
{
    public class AdminEntity
    {
        public Guid UserId { get; set; }
        public DateTime LastActionAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public UserEntity User { get; set; }
    }
}
