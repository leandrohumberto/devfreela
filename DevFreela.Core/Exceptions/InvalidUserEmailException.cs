namespace DevFreela.Core.Exceptions
{
    public class InvalidUserEmailException : Exception
    {
        public string Email { get; }

        public InvalidUserEmailException(string email, string? message = default) : base(message)
        {
            Email = email;
        }
    }
}