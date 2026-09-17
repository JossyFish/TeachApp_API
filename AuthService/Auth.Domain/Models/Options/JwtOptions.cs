namespace Auth.Domain.Models.Options
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public string RefreshSecretKey { get; set; } = string.Empty;
        public int ExpiresMinutes { get; set; }
        public int RefreshExpiresDays { get; set; }
    }
}
