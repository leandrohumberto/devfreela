using DevFreela.Application.ViewModels;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Queries.GetSkillById
{
#pragma warning disable CS8603 // Possible null reference return.
    public class GetSkillByIdQueryHandler : IRequestHandler<GetSkillByIdQuery, SkillViewModel>
    {
        private DevFreelaDbContext _dbContext;

        public GetSkillByIdQueryHandler(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SkillViewModel> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
        {
            var skill = await _dbContext.Skills.Where(s => s.Id == request.Id).SingleOrDefaultAsync(cancellationToken);
            return skill != null ? new SkillViewModel(skill.Id, skill.Description, skill.Disabled) : null;
        }
    }
}
