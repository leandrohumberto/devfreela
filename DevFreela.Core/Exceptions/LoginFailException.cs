namespace DevFreela.Core.Exceptions
{
    public class LoginFailException : Exception
    {
        public string Email { get; }

        public string Password { get; }

        public LoginFailException(string email, string password, string? message = default) : base(message)
        {
            Email = email;
            Password = password;
        }
    }
}