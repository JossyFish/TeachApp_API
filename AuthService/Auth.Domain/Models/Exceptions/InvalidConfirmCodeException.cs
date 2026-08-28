namespace Auth.Domain.Models.Exceptions
{
    public class InvalidConfirmCodeException : Exception
    {
        public string ConfirmCode { get; }

        public InvalidConfirmCodeException(string confirmCode)
            : base($"Invalid confirmCode '{confirmCode}'")
        {
            ConfirmCode = confirmCode;
        }
    }
}
