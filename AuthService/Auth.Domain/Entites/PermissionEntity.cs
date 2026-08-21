namespace Auth.Domain.Entites
{
    public class PermissionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<RoleEntity> Roles { get; set; } = [];
    }
}
