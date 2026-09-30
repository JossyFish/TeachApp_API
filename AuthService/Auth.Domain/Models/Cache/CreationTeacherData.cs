namespace Auth.Domain.Models.Cache
{
    public class CreationTeacherData
    {

        public Guid Id { get; }
        public string Name { get; }
        public string LastName { get; }
        public string Email { get; }
        public string PasswordHash { get; }
        public string ConfirmationCode { get; }

        public List<int> ExpertiseIds { get; }
        public string Experience { get; }
        public string Bio { get; }

        public int SubscriptionPlanId { get; }

        public string CardNumber { get; }
        public string CardExpiry { get; }
        public string CardCvc { get; }

        public CreationTeacherData(
            Guid id,
            string name,
            string lastName,
            string email,
            string passwordHash,
            string confirmationCode,
            List<int> expertiseIds,
            string experience,
            string bio,
            int subscriptionPlanId,
            string cardNumber,
            string cardExpiry,
            string cardCvc)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            ConfirmationCode = confirmationCode;
            ExpertiseIds = expertiseIds;
            Experience = experience;
            Bio = bio;
            SubscriptionPlanId = subscriptionPlanId;
            CardNumber = cardNumber;
            CardExpiry = cardExpiry;
            CardCvc = cardCvc;
        }
    }
}
