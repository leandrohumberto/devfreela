namespace DevFreela.Core.Exceptions
{
    public class InvalidProjectException : Exception
    {
        public int IdProject { get; }

        public InvalidProjectException(int idProject, string? message = default) : base(message)
        {
            IdProject = idProject;
        }
    }
}
