namespace DevFreela.Core.Entities
{
    public class ProjectComment : BaseEntity
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ProjectComment(string content, int idProject, int idUser)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Content = content;
            this.IdProject = idProject;
            this.IdUser = idUser;
        }

        public string Content { get; private set; }

        public int IdProject { get; private set; }

        public Project Project { get; private set; }

        public int IdUser { get; private set; }

        public User User { get; private set; }

        public DateTime CreaedAt { get; private set; }
    }
}