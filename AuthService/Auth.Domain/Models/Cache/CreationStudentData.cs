namespace Auth.Domain.Models.Cache
{
    public class CreationStudentData
    {
        public Guid Id { get; }
        public string Name { get; }
        public string LastName { get; }
        public string Email { get; }
        public string PasswordHash { get; }
        public string ConfirmationCode { get; }

        public CreationStudentData(Guid id, string name, string lastName, string email, string passwordHash, string confirmationCode)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            ConfirmationCode = confirmationCode;
        }
    }
}
