using DevFreela.Core.Entities;

namespace DevFreela.Infrastructure.Persistence
{
    public class DevFreelaDbContext
    {
        public DevFreelaDbContext()
        {
            Projects = new List<Project>
            {
                new Project("Meu project ASP.NET Core 1", "Minha descrição de projeto 1", 1, 2, 10000),
                new Project("Meu project ASP.NET Core 2", "Minha descrição de projeto 2", 1, 3, 20000),
                new Project("Meu project ASP.NET Core 3", "Minha descrição de projeto 3", 1, 2, 30000),
            };

            Users = new List<User>
            {
                new User("Emanuel Sales", "manusales@mail.com", new DateTime(2000, 1, 1)),
                new User("Ramón Gil", "ramongil@mail.com", new DateTime(2001, 2, 2)),
                new User("Otto Gualberto", "otgualberto@mail.com", new DateTime(2002, 3, 3)),
            };

            Skills = new List<Skill>
            {
                new Skill(".NET Core"),
                new Skill("C#"),
                new Skill("SQL"),
            };

            Comments = new List<ProjectComment>
            {
                new ProjectComment("Comentário do projeto", 1, 1),
                new ProjectComment("Comentário do projeto", 1, 2),
                new ProjectComment("Comentário do projeto", 2, 1),
            };
        }

        public List<Project> Projects { get; set; }

        public List<User> Users { get; set; }

        public List<Skill> Skills { get; set; }

        public List<ProjectComment> Comments { get; set; }
    }
}
