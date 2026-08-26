namespace Auth.Application.Interfaces
{
    public interface INumberProcessor
    {
        string GenerateConfirmCode();
        string Generate(string password);
        bool Verify(string password, string hashedPassword);

    }
}