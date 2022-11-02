using DevFreela.Core.Enums;
using System.Linq;

namespace DevFreela.Application.ViewModels
{
    public class UserDetailViewModel
    {
        public UserDetailViewModel(string fullName, string email, DateTime birthDate, RoleEnum role, bool active, IEnumerable<SkillViewModel> skills)
        {
            FullName = fullName;
            Email = email;
            BirthDate = birthDate;
            Active = active;
            Role = role;

            Skills = Enumerable.Empty<SkillViewModel>();
            foreach (var skill in skills) Skills = Skills.Append(skill);
        }

        public string FullName { get; private set; }

        public string Email { get; private set; }

        public DateTime BirthDate { get; private set; }

        public RoleEnum Role { get; private set; }

        public bool Active { get; private set; }

        public IEnumerable<SkillViewModel> Skills { get; private set; }
    }
}