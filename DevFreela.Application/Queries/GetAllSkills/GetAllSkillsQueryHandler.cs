using DevFreela.Application.ViewModels;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Queries.GetAllSkills
{
    public class GetAllSkillsQueryHandler : IRequestHandler<GetAllSkillsQuery, IEnumerable<SkillViewModel>>
    {
        private readonly DevFreelaDbContext _dbContext;

        public GetAllSkillsQueryHandler(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SkillViewModel>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
            => await _dbContext.Skills.Select(s => new SkillViewModel(s.Id, s.Description, s.Disabled)).ToListAsync(cancellationToken);
    }
}
