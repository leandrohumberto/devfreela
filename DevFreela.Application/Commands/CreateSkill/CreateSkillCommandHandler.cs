using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Commands.CreateSkill
{
    public class CreateSkillCommandHandler : IRequestHandler<CreateSkillCommand, int?>
    {
        private readonly DevFreelaDbContext _dbContext;

        public CreateSkillCommandHandler(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int?> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
        {
            var hasRepetitions = await _dbContext.Skills.AnyAsync(s => s.Description == request.Description, cancellationToken);

            if (!hasRepetitions)
            {
                var skill = new Skill(request.Description);
                _dbContext.Skills.Add(skill);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return skill.Id;
            }

            return null;
        }
    }
}
