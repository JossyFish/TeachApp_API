namespace Auth.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? CardLastDigits { get; set; }
        public string? CardBrand { get; set; }
        public List<string?> Roles { get; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;


        public User() { }

        public User(Guid id, string name, string lastName, string email, string passwordHash,
            List<string>? roles, string? cardLastDigits, string? cardBrand, bool isActive,
            DateTime createdAt, DateTime updatedAt, DateTime lastLogin)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            Roles = roles ?? new List<string>();
            CardLastDigits = cardLastDigits;
            CardBrand = cardBrand;
            IsActive = isActive;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            LastLogin = lastLogin;
        }
    }

}