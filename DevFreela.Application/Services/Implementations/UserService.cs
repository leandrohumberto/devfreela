using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Services.Implementations
{
#pragma warning disable CS8603 // Possible null reference return.
    public class UserService : IUserService
    {
        private readonly DevFreelaDbContext _dbContext;

        public UserService(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Create(CreateUserInputModel model)
        {
            var user = new User(model.FullName, model.Email, model.BirthDate.Date);
            
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            if (model.Skills != null)
            {
                foreach (var idSkill in model.Skills)
                {
                    if (_dbContext.Skills.Any(s => s.Id == idSkill))
                    {
                        var skill = _dbContext.Skills.Single(s => s.Id == idSkill);
                        var userSkill = new UserSkill(user.Id, skill.Id);
                        user.UserSkills.Add(userSkill);
                    }
                }
            }
            
            _dbContext.SaveChanges();

            return user.Id;
        }

        public UserDetailViewModel GetById(int id)
        {
            var user = _dbContext.Users
                .Include(p => p.UserSkills.Where(s => s.Skill.Disabled != true))
                .ThenInclude(p => p.Skill)
                .SingleOrDefault(p => p.Id == id);
            if (user == null) return null;

            var skillViewModel = new List<SkillViewModel>();

            if (user.UserSkills != null)
            {
                foreach (var userSkill in user.UserSkills)
                {
                    skillViewModel.Add(new SkillViewModel(userSkill.Skill.Id, userSkill.Skill.Description, userSkill.Skill.Disabled));
                }
            }

            return new UserDetailViewModel(user.FullName, user.Email, user.BirthDate, user.Active, skillViewModel);
        }
    }
}
