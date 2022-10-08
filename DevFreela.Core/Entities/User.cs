namespace DevFreela.Core.Entities
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class User : BaseEntity
    {
        public User(string fullName, string email, DateTime birthDate)
        {
            FullName = fullName;
            Email = email;
            BirthDate = birthDate;
            CreatedAt = DateTime.Now;
            Active = true;

            UserSkills = new List<UserSkill>();
            OwnedProjects = new List<Project>();
            FreelanceProjects = new List<Project>();
        }

        public string FullName { get; private set; }

        public string Email { get; private set; }

        public DateTime BirthDate { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public List<UserSkill> UserSkills { get; private set; }

        public List<Project> OwnedProjects { get; private set; }

        public List<Project> FreelanceProjects { get; private set; }

        public List<ProjectComment> Comments { get; private set; }

        public bool Active { get; private set; }
    }
}
