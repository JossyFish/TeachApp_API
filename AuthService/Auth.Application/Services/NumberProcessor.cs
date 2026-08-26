using Auth.Application.Interfaces;
using System.Security.Cryptography;

namespace Auth.Application.Services
{
    public class NumberProcessor : INumberProcessor
    {
        public string GenerateConfirmCode()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var randomNumber = new byte[4];
                rng.GetBytes(randomNumber);
                var randomValue = BitConverter.ToUInt32(randomNumber, 0) % 1000000;

                return (randomValue == 0 ? 1 : randomValue).ToString("D6");
            }
        }

        public string Generate(string password) =>
           BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public bool Verify(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);

    }
}
