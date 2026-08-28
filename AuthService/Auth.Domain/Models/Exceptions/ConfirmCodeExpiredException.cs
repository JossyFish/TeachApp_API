namespace Auth.Domain.Models.Exceptions
{
    public class ConfirmCodeExpiredException : Exception
    {
        public string ConfirmCode { get; }

        public ConfirmCodeExpiredException(string confirmCode)
            : base($"ConfirmCode '{confirmCode}' is expired")
        {
            ConfirmCode = confirmCode;
        }
    }
}
