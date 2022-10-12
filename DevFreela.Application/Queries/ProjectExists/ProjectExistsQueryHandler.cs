using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Queries.ProjectExists
{
    public class ProjectExistsQueryHandler : IRequestHandler<ProjectExistsQuery, bool>
    {
        private readonly DevFreelaDbContext _dbContext;

        public ProjectExistsQueryHandler(DevFreelaDbContext dbContext) => _dbContext = dbContext;

        public async Task<bool> Handle(ProjectExistsQuery request, CancellationToken cancellationToken)
            => await _dbContext.Projects.AnyAsync(p => p.Id == request.Id, cancellationToken);
    }
}
