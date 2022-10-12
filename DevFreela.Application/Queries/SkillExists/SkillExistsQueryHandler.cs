using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Queries.SkillExists
{
    public class SkillExistsQueryHandler : IRequestHandler<SkillExistsQuery, bool>
    {
        private readonly DevFreelaDbContext _dbContext;

        public SkillExistsQueryHandler(DevFreelaDbContext dbContext) => _dbContext = dbContext;

        public async Task<bool> Handle(SkillExistsQuery request, CancellationToken cancellationToken)
            => await _dbContext.Skills.AnyAsync(p => p.Id == request.Id, cancellationToken);
    }
}
