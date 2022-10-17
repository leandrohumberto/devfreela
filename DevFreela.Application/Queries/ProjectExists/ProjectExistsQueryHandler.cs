using DevFreela.Core.Repositories;
using MediatR;

namespace DevFreela.Application.Queries.ProjectExists
{
    public class ProjectExistsQueryHandler : IRequestHandler<ProjectExistsQuery, bool>
    {
        private readonly IProjectRepository _repository;

        public ProjectExistsQueryHandler(IProjectRepository dbContext) => _repository = dbContext;

        public async Task<bool> Handle(ProjectExistsQuery request, CancellationToken cancellationToken)
            => await _repository.ExistsAsync(request.Id, cancellationToken);
    }
}
