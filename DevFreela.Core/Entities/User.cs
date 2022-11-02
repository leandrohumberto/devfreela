using DevFreela.Core.Enums;

namespace DevFreela.Core.Entities
{
    public class User : BaseEntity
    {
        public User(string fullName, string email, DateTime birthDate, string password, RoleEnum role)
        {
            FullName = fullName;
            Email = email;
            BirthDate = birthDate;
            Password = password;
            Role = role;
            CreatedAt = DateTime.Now;
            Active = true;

            UserSkills = new List<UserSkill>();
            OwnedProjects = new List<Project>();
            FreelanceProjects = new List<Project>();
            Comments = new List<ProjectComment>();
        }

        public string FullName { get; private set; }

        public string Email { get; private set; }

        public DateTime BirthDate { get; private set; }

        public string Password { get; private set; }

        public RoleEnum Role { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public List<UserSkill> UserSkills { get; private set; }

        public List<Project> OwnedProjects { get; private set; }

        public List<Project> FreelanceProjects { get; private set; }

        public List<ProjectComment> Comments { get; private set; }

        public bool Active { get; private set; }
    }
}
