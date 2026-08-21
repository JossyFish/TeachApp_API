namespace Auth.Domain.Entites
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PermissionEntity> Permissions { get; set; } = [];
        public ICollection<UserEntity> Users { get; set; } = [];
    }
}
