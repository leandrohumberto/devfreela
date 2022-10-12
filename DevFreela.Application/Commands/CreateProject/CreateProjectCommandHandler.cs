using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using MediatR;

namespace DevFreela.Application.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
    {

        private readonly DevFreelaDbContext _dbContext;

        public CreateProjectCommandHandler(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project(request.Title,
                request.Description,
                request.IdClient,
                request.IdFreelancer,
                request.TotalCost);

            _ = await _dbContext.Projects.AddAsync(project, cancellationToken);
            _ = await _dbContext.SaveChangesAsync(cancellationToken);

            return project.Id;
        }
    }
}
