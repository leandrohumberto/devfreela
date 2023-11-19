namespace DevFreela.Core.Exceptions
{
    public class InvalidUserException : Exception
    {
        public int IdUser { get; }

        public InvalidUserException(int idUser, string? message = default) : base(message)
        {
            IdUser = idUser;
        }
    }
}
