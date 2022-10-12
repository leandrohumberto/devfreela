using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;

namespace DevFreela.Application.Services.Implementations
{
#pragma warning disable CS8603 // Possible null reference return.
    public class SkillService : ISkillService
    {
        private readonly DevFreelaDbContext _dbContext;

        public SkillService(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int? Create(CreateSkillInputModel inputModel)
        {
            if (string.IsNullOrWhiteSpace(inputModel?.Description))
            {
                throw new ArgumentException($"'{nameof(inputModel.Description)}' cannot be null or empty.",
                    nameof(inputModel.Description));
            }

            var hasRepetitions = _dbContext.Skills.Any(s => s.Description == inputModel.Description);

            if (!hasRepetitions)
            {
                var skill = new Skill(inputModel.Description);
                _dbContext.Skills.Add(skill);
                _dbContext.SaveChanges();
                return skill.Id;
            }

            return null;
        }

        public void Delete(int id)
        {
            var skill = _dbContext.Skills.Where(s => s.Id == id).First();
            skill?.Disable();
            _dbContext.SaveChanges();
        }

        public IEnumerable<SkillViewModel> GetAll()
            => _dbContext.Skills.Select(s => new SkillViewModel(s.Id, s.Description, s.Disabled));

        public SkillViewModel GetById(int id)
        {
            var skill = _dbContext.Skills.Where(s => s.Id == id).SingleOrDefault();

            return skill != null ? new SkillViewModel(skill.Id, skill.Description, skill.Disabled) : null;
        }

        public void Update(int id, UpdateSkillInputModel inputModel)
        {
            if (string.IsNullOrWhiteSpace(inputModel?.Description))
            {
                throw new ArgumentException($"'{nameof(inputModel.Description)}' cannot be null or empty.",
                    nameof(inputModel.Description));
            }

            var hasRepetitions = _dbContext.Skills.Any(s => s.Id != id && s.Description == inputModel.Description);

            if (!hasRepetitions)
            {
                var skill = _dbContext.Skills.Where(s => s.Id == id).First();
                skill?.Update(inputModel.Description, inputModel.Disabled);
                _dbContext.SaveChanges();
            }
        }
    }
}
